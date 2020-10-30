namespace Byndyusoft.Extensions.Configuration.Vault.Vault
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using VaultSharp;
    using VaultSharp.V1.SecretsEngines;

    internal static class VaultClientExtensions
    {
        public static async Task<bool> IsInitializedAsync(this VaultClient client)
        {
            try
            {
                var status = await client.V1.System.GetHealthStatusAsync().ConfigureAwait(false);
                return status.Initialized;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
        
        public static async Task<Engine> CreateEngineAsync(this VaultClient client, string engine, VaultKeyValueEngineVersion version)
        {
            await client.V1.System.MountSecretBackendAsync(
                new SecretsEngine
                {
                    Type = MapType(version),
                    Path = engine
                }).ConfigureAwait(false);
            return new Engine(client, engine, version);
        }

        public static async Task RemoveEngineAsync(this VaultClient client, string engine)
        {
            await client.V1.System.UnmountSecretBackendAsync(engine).ConfigureAwait(false);
        }

        private static SecretsEngineType MapType(VaultKeyValueEngineVersion version)
        {
            switch (version)
            {
                case VaultKeyValueEngineVersion.V1:
                    return SecretsEngineType.KeyValueV1;
                case VaultKeyValueEngineVersion.V2:
                    return SecretsEngineType.KeyValueV2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(version));
            }
        }
    }
}