using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp;

namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    public class VaultKeyValueEngineV2 : VaultKeyValueEngine
    {
        public VaultKeyValueEngineV2(VaultClient client, VaultKeyValueEngineConfigurationSource source) : base(client, source)
        {
        }

        protected override async Task<IEnumerable<string>> ReadSecretKeysAsync()
        {
            var list = await Client.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync(string.Empty, Name).ConfigureAwait(false);
            return list.Data.Keys;
        }

        protected override async Task<IDictionary<string, object>> ReadSecretAsync(string path)
        {
            var secret = await Client.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: Name).ConfigureAwait(false);

            return secret.Data.Data;
        }
    }
}