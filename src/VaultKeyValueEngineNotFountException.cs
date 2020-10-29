using System;

namespace Byndyusoft.Extensions.Configuration.Vault
{
    public class VaultKeyValueEngineNotFountException : Exception
    {
        public VaultKeyValueEngineNotFountException(string engineName)
        {
            EngineName = engineName;
        }

        public string EngineName { get; }
    }
}