using System;
using Microsoft.Extensions.Configuration;
using VaultSharp;

namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    public class VaultKeyValueEngineFactory
    {
        public IVaultKeyValueEngine CreateEngine(VaultConfigurationSource source)
        {
            var clientSettings = source.AsVaultClientSettings();
            var client = new VaultClient(clientSettings);

            switch (source.Engine.Version)
            {
                case VaultEngineVersion.V1:
                    return new VaultKeyValueEngineV1(client, source);
                case VaultEngineVersion.V2:
                    return new VaultKeyValueEngineV2(client, source);
                default:
                    throw new ArgumentOutOfRangeException(nameof(VaultEngineVersion));
            }

        }
    }
}