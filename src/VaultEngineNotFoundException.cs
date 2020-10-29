using System;

namespace Byndyusoft.Extensions.Configuration.Vault
{
    public class VaultEngineNotFoundException : Exception
    {
        public VaultEngineNotFoundException(string engineName)
        {
            EngineName = engineName;
        }

        public string EngineName { get; }
    }
}