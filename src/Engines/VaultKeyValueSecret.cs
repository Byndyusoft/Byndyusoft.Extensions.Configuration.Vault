using System.Collections.Generic;

namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    public class VaultKeyValueSecret
    {
        public string Key { get; set; }

        public IDictionary<string, object> Values { get; set; }
    }
}