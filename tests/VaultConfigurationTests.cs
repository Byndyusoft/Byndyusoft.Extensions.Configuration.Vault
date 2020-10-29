namespace Byndyusoft.Extensions.Configuration.Vault
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Api;
    using Microsoft.Extensions.Configuration;
    using VaultSharp;
    using VaultSharp.V1.AuthMethods;
    using VaultSharp.V1.AuthMethods.Token;
    using Xunit;

    public class VaultConfigurationTests : IAsyncLifetime, IClassFixture<VaultConfigurationTests.VaultFixture>
    {
        public class VaultFixture : IDisposable
        {
            public readonly IVaultApi Api;

            public VaultFixture()
            {
                Api = new DockerVaultApi();
                //Api = new ExtenalVaultApi("http://localhost:8200", "s.aZxzEX9aDL6GWIudspV5l3wl");
                Api.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }

            public void Dispose()
            {
                Api.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        private readonly VaultClient _vaultClient;
        private readonly IAuthMethodInfo _authMethod;
        private readonly string _uri;
        private readonly string _engineName;

        public VaultConfigurationTests(VaultFixture fixture)
        {
            _authMethod = new TokenAuthMethodInfo(fixture.Api.Token);
            _uri = fixture.Api.Url;
            _engineName = "kv";
            _vaultClient = new VaultClient(new VaultClientSettings(_uri, _authMethod));
        }

        public async Task InitializeAsync()
        {
            await _vaultClient.RemoveEngine(_engineName);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        private IConfiguration Configure(Action<VaultConfigurationSource> configureSource)
        {
            return new ConfigurationBuilder()
                .AddVault(_uri, _authMethod, _engineName, configureSource)
                .Build();
        }

        [Theory]
        [InlineData(VaultEngineVersion.V1)]
        [InlineData(VaultEngineVersion.V2)]
        public async Task VaultConfiguration_ReadsSimpleValue(VaultEngineVersion version)
        {
            // Arrange
            var engine = await _vaultClient.CreateEngine(_engineName, version);
            var secretValue = new Dictionary<string, object> {{"key", "value"}};
            await engine.AddSecretAsync("secret", secretValue);

            // Act
            var config = Configure(vault => { vault.Engine.Version = version; });

            // Assert
            Assert.Equal("value", config.GetValue<string>("secret:key"));
        }

        [Theory]
        [InlineData(VaultEngineVersion.V1)]
        [InlineData(VaultEngineVersion.V2)]
        public async Task VaultConfiguration_ReadsComplexValue(VaultEngineVersion version)
        {
            // Arrange
            var engine = await _vaultClient.CreateEngine(_engineName, version);
            var secretValue = new Dictionary<string, object> {{"key1", "value1"}, {"key2", "value2"}};
            await engine.AddSecretAsync("secret", secretValue);

            // Act
            var config = Configure(vault => { vault.Engine.Version = version; });

            // Assert
            Assert.Equal("value1", config.GetValue<string>("secret:key1"));
            Assert.Equal("value2", config.GetValue<string>("secret:key2"));
        }

        [Theory]
        [InlineData(VaultEngineVersion.V1)]
        [InlineData(VaultEngineVersion.V2)]
        public async Task VaultConfiguration_ReadsJsonValue(VaultEngineVersion version)
        {
            // Arrange
            var engine = await _vaultClient.CreateEngine(_engineName, version);

            var json = new
                       {
                           key1 = "value1",
                           key2 = "value2"
                       };
            var secretValue = new Dictionary<string, object> {{"json", json}};
            await engine.AddSecretAsync("secret", secretValue);

            // Act
            var config = Configure(vault => { vault.Engine.Version = version; });

            // Assert
            Assert.Equal("value1", config.GetValue<string>("secret:json:key1"));
            Assert.Equal("value2", config.GetValue<string>("secret:json:key2"));
        }

        [Theory]
        [InlineData(VaultEngineVersion.V1)]
        [InlineData(VaultEngineVersion.V2)]
        public void VaultConfiguration_NoEngine_ReadsNothing(VaultEngineVersion version)
        {
            // Act
            var config = Configure(
                vault =>
                {
                    vault.Engine.Version = version;
                    vault.Optional = true;
                });

            // Assert
            Assert.Null(config.GetValue<string>("secret"));
        }

        [Theory]
        [InlineData(VaultEngineVersion.V1)]
        [InlineData(VaultEngineVersion.V2)]
        public void VaultConfiguration_NoEngine_ThrowsIfNotOptional(VaultEngineVersion version)
        {
            // Act
            var exception = Assert.Throws<VaultEngineNotFoundException>(
                () => Configure(
                    vault =>
                    {
                        vault.Engine.Version = version;
                        vault.Optional = false;
                    }));

            // Assert
            Assert.Equal(_engineName, exception.EngineName);
        }
    }
}