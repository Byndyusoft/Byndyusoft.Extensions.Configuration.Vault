namespace Byndyusoft.Extensions.Configuration.Vault.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    
    public class DockerVaultApi : IVaultApi
    {
        private readonly string _port;
        private readonly DockerClient _docker;
        private string _containerId;

        public DockerVaultApi()
        {
            _docker = new DockerClientConfiguration().CreateClient();
            _port = FreeTcpPort().ToString();
            Token = Guid.NewGuid().ToString();
        }

        public string Url => $"http://localhost:{_port}";

        public string Token { get; }

        public async ValueTask StartAsync()
        {
            await _docker.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = "vault",
                    Tag = "latest"
                }, 
                null, new Progress<JSONMessage>());

            var container = await _docker.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Image = "vault",
                    AttachStderr = true,
                    AttachStdin = true,
                    AttachStdout = true,
                    Env = new[] {$"VAULT_DEV_ROOT_TOKEN_ID={Token}"},
                    Tty = true,
                    ExposedPorts = new Dictionary<string, EmptyStruct>
                                   {
                                       {"8200", default}
                                   },
                    HostConfig = new HostConfig
                                 {
                                     PortBindings = new Dictionary<string, IList<PortBinding>>
                                                    {
                                                        {
                                                            "8200",
                                                            new[]
                                                            {
                                                                new PortBinding
                                                                {
                                                                    HostPort = _port
                                                                }
                                                            }
                                                        }
                                                    }
                                 }
                });
            _containerId = container.ID;
            try
            {
                await _docker.Containers.StartContainerAsync(_containerId, null);
                await WaitStartingAsync(TimeSpan.FromSeconds(5));
            }
            catch
            {
                await StopAsync();
            }
        }

        private async Task WaitStartingAsync(TimeSpan timeout)
        {
            var parameters = new ContainerLogsParameters
                             {
                                 ShowStdout = true,
                                 ShowStderr = true
                             };
            var sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < timeout)
            {
                var logs = await _docker.Containers.GetContainerLogsAsync(_containerId, true, parameters);
                var (stdout, _) = await logs.ReadOutputToEndAsync(CancellationToken.None);

                if (stdout.Contains("Vault server started!"))
                {
                    return;
                }

                await Task.Delay(100);
            }

            throw new TimeoutException();
        }

        private async ValueTask StopAsync()
        {
            if (_containerId == null)
                return;

            await _docker.Containers.RemoveContainerAsync(_containerId,
                new ContainerRemoveParameters
                {
                    Force = true
                });
            _containerId = null;
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync();
        }

        private static int FreeTcpPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}