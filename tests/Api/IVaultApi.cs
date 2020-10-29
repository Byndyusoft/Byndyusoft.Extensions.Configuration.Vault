namespace Byndyusoft.Extensions.Configuration.Vault.Api
{
    using System;
    using System.Threading.Tasks;

    public interface IVaultApi : IAsyncDisposable
    {
        ValueTask StartAsync();

        string Token { get; }

        string Url { get; }
    }
}