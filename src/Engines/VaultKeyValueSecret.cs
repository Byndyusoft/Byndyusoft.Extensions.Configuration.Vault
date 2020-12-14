namespace Byndyusoft.Extensions.Configuration.Vault.Engines
{
    using System.Collections.Generic;

    public class VaultKeyValueSecret
    {
        public string Key { get; set; }

        public IDictionary<string, object> Values { get; set; }
    }
}