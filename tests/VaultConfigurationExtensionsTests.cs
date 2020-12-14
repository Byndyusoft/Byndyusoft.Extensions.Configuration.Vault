namespace Byndyusoft.Extensions.Configuration.Vault
{
    using System;
    using Microsoft.Extensions.Configuration;
    using VaultSharp.V1.AuthMethods;
    using VaultSharp.V1.AuthMethods.Token;
    using Xunit;

    public class VaultConfigurationExtensionsTests
    {
        private readonly IAuthMethodInfo _authMethod = new TokenAuthMethodInfo("token");
        private readonly string _engine = "engine";
        private readonly string _url = "http://localhost:8200";

        [Fact]
        public void AddVault_ThrowsIfBuilderIsNull()
        {
            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => ((ConfigurationBuilder) null).AddVault(_url, _authMethod, _engine));
            Assert.Equal("builder", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddVault_ThrowsIfServerUrlIsNullOrEmpty(string url)
        {
            // Arrange
            var builder = new ConfigurationBuilder();

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => builder.AddVault(url, _authMethod, _engine));
            Assert.Equal("vaultServerUriWithPort", ex.ParamName);
        }

        [Fact]
        public void AddVault_ThrowsIfAuthMethodIsNull()
        {
            // Arrange
            var builder = new ConfigurationBuilder();

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => builder.AddVault(_url, null, _engine));
            Assert.Equal("authMethodInfo", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddVault_ThrowsIfEngineNameIsNullOrEmpty(string engineName)
        {
            // Arrange
            var builder = new ConfigurationBuilder();

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => builder.AddVault(_url, _authMethod, engineName));
            Assert.Equal("engineName", ex.ParamName);
        }
    }
}