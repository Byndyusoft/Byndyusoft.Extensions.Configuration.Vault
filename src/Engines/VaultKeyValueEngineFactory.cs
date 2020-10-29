using System;
using Microsoft.Extensions.Configuration;
using VaultSharp;

namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    public class VaultKeyValueEngineFactory
    {
        public IVaultKeyValueEngine CreateEngine(VaultKeyValueEngineConfigurationSource source)
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