namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IVaultKeyValueEngine
    {
        Task<IReadOnlyCollection<VaultKeyValueSecret>> ReadSecretsAsync();
    }
}