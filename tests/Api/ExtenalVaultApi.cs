namespace Byndyusoft.Extensions.Configuration.Vault.Api
{
    using System.Threading.Tasks;

    public class ExtenalVaultApi : IVaultApi
    {
        public ExtenalVaultApi(string url, string token)
        {
            Url = url;
            Token = token;
        }

        public string Token { get; }

        public string Url { get; }

        public ValueTask DisposeAsync() => default;

        public ValueTask StartAsync() => default;
    }
}