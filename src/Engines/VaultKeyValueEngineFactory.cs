namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    using System;
    using Microsoft.Extensions.Configuration;
    using VaultSharp;

    public class VaultKeyValueEngineFactory
    {
        public IVaultKeyValueEngine CreateEngine(VaultConfigurationSource source)
        {
            var clientSettings = source.AsVaultClientSettings();
            var client = new VaultClient(clientSettings);

            switch (source.Engine.Version)
            {
                case VaultKeyValueEngineVersion.V1:
                    return new VaultKeyValueEngineV1(client, source);
                case VaultKeyValueEngineVersion.V2:
                    return new VaultKeyValueEngineV2(client, source);
                default:
                    throw new ArgumentOutOfRangeException(nameof(VaultKeyValueEngineVersion));
            }
        }
    }
}