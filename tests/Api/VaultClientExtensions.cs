namespace Byndyusoft.Extensions.Configuration.Vault.Api
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using VaultSharp;
    using VaultSharp.V1.SecretsEngines;

    public static class VaultClientExtensions
    {
        public static async Task<Engine> CreateEngine(this VaultClient client, string engine, VaultEngineVersion version)
        {
            await client.V1.System.MountSecretBackendAsync(
                new SecretsEngine
                {
                    Type = MapType(version),
                    Path = engine
                });
            return new Engine(client, engine, version);
        }

        public static async Task RemoveEngine(this VaultClient client, string engine)
        {
            await client.V1.System.UnmountSecretBackendAsync(engine);
        }

        private static SecretsEngineType MapType(VaultEngineVersion version)
        {
            switch (version)
            {
                case VaultEngineVersion.V1:
                    return SecretsEngineType.KeyValueV1;
                case VaultEngineVersion.V2:
                    return SecretsEngineType.KeyValueV2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(version));
            }
        }
    }
}