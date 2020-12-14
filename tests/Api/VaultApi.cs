namespace Byndyusoft.Extensions.Configuration.Vault.Api
{
    using System;

    public static class VaultApi
    {
        public static IVaultApi Create()
        {
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                var address = Environment.GetEnvironmentVariable("VAULT_ADDRESS");
                var token = Environment.GetEnvironmentVariable("VAULT_TOKEN");

                return new ExtenalVaultApi(address, token);
            }

            return new DockerVaultApi();
        }
    }
}