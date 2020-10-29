// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration
{
    using System;
    using Byndyusoft.Extensions.Configuration.Vault;
    using VaultSharp.V1.AuthMethods;

    /// <summary>
    /// Extension methods for adding <see cref="VaultConfigurationProvider" />.
    /// </summary>
    public static class VaultConfigurationExtensions
    {
        /// <summary>
        /// Adds a HashiCorp Vault configuration source to <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" /> to add to.</param>
        /// <param name="vaultServerUriWithPort">The Vault Server Uri with port.</param>
        /// <param name="authMethodInfo">The auth method to be used to acquire a vault token.</param>
        /// <param name="engineName">The name of the KeyValue secrets engine.</param>
        public static IConfigurationBuilder AddVault(
            this IConfigurationBuilder builder,
            string vaultServerUriWithPort,
            IAuthMethodInfo authMethodInfo,
            string engineName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(vaultServerUriWithPort))
                throw new ArgumentException(Resources.Error_InvalidVaultServerUrl, nameof(vaultServerUriWithPort));
            if (authMethodInfo == null) throw new ArgumentNullException(nameof(authMethodInfo));
            if (string.IsNullOrWhiteSpace(engineName))
                throw new ArgumentException(Resources.Error_InvalidEngineName, nameof(engineName));

            return builder.AddVault(
                source =>
                {
                    source.Uri = vaultServerUriWithPort;
                    source.AuthMethod = authMethodInfo;
                    source.Engine.Name = engineName;
                });
        }

        /// <summary>
        /// Adds a HashiCorp Vault configuration source to <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" /> to add to.</param>
        /// <param name="vaultServerUriWithPort">The Vault Server Uri with port.</param>
        /// <param name="authMethodInfo">The auth method to be used to acquire a vault token.</param>
        /// <param name="engineName">The name of the KeyValue secrets engine.</param>
        /// <param name="configureSource">Configures the source.</param>
        public static IConfigurationBuilder AddVault(
            this IConfigurationBuilder builder,
            string vaultServerUriWithPort,
            IAuthMethodInfo authMethodInfo,
            string engineName,
            Action<VaultConfigurationSource> configureSource)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(vaultServerUriWithPort))
                throw new ArgumentException(Resources.Error_InvalidVaultServerUrl, nameof(vaultServerUriWithPort));
            if (authMethodInfo == null) throw new ArgumentNullException(nameof(authMethodInfo));
            if (string.IsNullOrWhiteSpace(engineName))
                throw new ArgumentException(Resources.Error_InvalidEngineName, nameof(engineName));

            return builder.AddVault(
                source =>
                {
                    source.Uri = vaultServerUriWithPort;
                    source.AuthMethod = authMethodInfo;
                    source.Engine.Name = engineName;
                    configureSource(source);
                });
        }

        /// <summary>
        /// Adds a HashiCorp Vault configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddVault(this IConfigurationBuilder builder,
            Action<VaultConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}