namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VaultSharp;
    using VaultSharp.Core;

    public abstract class VaultKeyValueEngine : IVaultKeyValueEngine
    {
        protected VaultKeyValueEngine(VaultClient client, VaultConfigurationSource source)
        {
            Client = client;
            Source = source;
            Name = source.Engine.Name;
        }

        protected string Name { get; }

        protected VaultClient Client { get; }

        private VaultConfigurationSource Source { get; }

        public virtual async Task<IReadOnlyCollection<VaultKeyValueSecret>> ReadSecretsAsync()
        {
            var secretKeys = await DoReadSecretKeysAsync().ConfigureAwait(false);

            var secrects = new List<VaultKeyValueSecret>();
            foreach (var secretKey in secretKeys)
            {
                var secret = await ReadSecretAsync(secretKey).ConfigureAwait(false);
                secrects.Add(new VaultKeyValueSecret
                             {
                                 Key = secretKey,
                                 Values = secret
                             });
            }

            return secrects;
        }

        protected abstract Task<IEnumerable<string>> ReadSecretKeysAsync();

        protected abstract Task<IDictionary<string, object>> ReadSecretAsync(string path);

        private async Task<IEnumerable<string>> DoReadSecretKeysAsync()
        {
            try
            {
                return await ReadSecretKeysAsync().ConfigureAwait(false);
            }
            catch (VaultApiException e) when (e.StatusCode == 404)
            {
                if (Source.Optional == false && await CheckIfExistsAsync().ConfigureAwait(false) == false)
                    throw new VaultEngineNotFoundException(Name);
            }

            return Enumerable.Empty<string>();
        }

        private async Task<bool> CheckIfExistsAsync()
        {
            try
            {
                var response = await Client.V1.System.GetSecretBackendConfigAsync(Name).ConfigureAwait(false);
                return response.Data != null;
            }
            catch (VaultApiException e) when (e.StatusCode == 400)
            {
                return false;
            }
        }
    }
}