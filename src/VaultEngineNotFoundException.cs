namespace Byndyusoft.Extensions.Configuration.Vault
{
    using System;

    public class VaultEngineNotFoundException : Exception
    {
        public VaultEngineNotFoundException(string engineName)
        {
            EngineName = engineName;
        }

        public string EngineName { get; }
    }
}