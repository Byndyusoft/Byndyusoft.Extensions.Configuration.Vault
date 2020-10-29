using System.Collections.Generic;
using System.Threading.Tasks;

namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    public interface IVaultKeyValueEngine
    {
        Task<IReadOnlyCollection<VaultKeyValueSecret>> ReadSecretsAsync();
    }
}