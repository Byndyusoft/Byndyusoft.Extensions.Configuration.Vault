namespace Byndyusoft.Extensions.Configuration.Vault.Api
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using VaultSharp;

    public class Engine
    {
        private readonly VaultClient _client;
        private readonly string _name;
        private readonly VaultEngineVersion _version;

        public Engine(VaultClient client, string name, VaultEngineVersion version)
        {
            _client = client;
            _name = name;
            _version = version;
        }

        public async Task AddSecretAsync(string secretName, Dictionary<string, object> values)
        {
            switch (_version)
            {
                case VaultEngineVersion.V1:
                    await _client.V1.Secrets.KeyValue.V1.WriteSecretAsync(secretName, values, _name);
                    return;
                case VaultEngineVersion.V2:
                    await _client.V1.Secrets.KeyValue.V2.WriteSecretAsync(secretName, values, mountPoint: _name);
                    return;
            }
        }
    }
}