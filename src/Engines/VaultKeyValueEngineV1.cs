namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VaultSharp;

    public class VaultKeyValueEngineV1 : VaultKeyValueEngine
    {
        public VaultKeyValueEngineV1(VaultClient client, VaultConfigurationSource source) : base(client, source)
        {
        }

        protected override async Task<IEnumerable<string>> ReadSecretKeysAsync()
        {
            var list = await Client.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync(string.Empty, Name).ConfigureAwait(false);
            return list.Data.Keys;
        }

        protected override async Task<IDictionary<string, object>> ReadSecretAsync(string path)
        {
            var secret = await Client.V1.Secrets.KeyValue.V1.ReadSecretAsync(path, Name).ConfigureAwait(false);
            return secret.Data;
        }
    }
}