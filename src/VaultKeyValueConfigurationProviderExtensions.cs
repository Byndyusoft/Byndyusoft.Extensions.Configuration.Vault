// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration
{
    using System;
    using Byndyusoft.Extensions.Configuration.Vault;
    using VaultSharp.V1.AuthMethods;

    /// <summary>
    /// Extension methods for adding <see cref="VaultKeyValueEngineConfigurationProvider" />.
    /// </summary>
    public static class VaultKeyValueConfigurationProviderExtensions
    {
        /// <summary>
        /// Adds a HashiCorp Vault configuration source to <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" /> to add to.</param>
        /// <param name="vaultServerUriWithPort">The Vault Server Uri with port.</param>
        /// <param name="authMethodInfo">The auth method to be used to acquire a vault token.</param>
        /// <param name="engineName">The name of the KeyValue secrets engine.</param>
        public static IConfigurationBuilder AddVaultKeyValueEngine(
            this IConfigurationBuilder builder,
            string vaultServerUriWithPort,
            IAuthMethodInfo authMethodInfo,
            string engineName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (vaultServerUriWithPort == null) throw new ArgumentNullException(nameof(vaultServerUriWithPort));
            if (authMethodInfo == null) throw new ArgumentNullException(nameof(authMethodInfo));
            if (engineName == null) throw new ArgumentNullException(nameof(engineName));

            return builder.AddVaultKeyValueEngine(s =>
                                                  {
                                                      s.Uri = vaultServerUriWithPort;
                                                      s.AuthMethod = authMethodInfo;
                                                      s.Engine.Name = engineName;
                                                  });
        }

        /// <summary>
        /// Adds a HashiCorp Vault configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddVaultKeyValueEngine(this IConfigurationBuilder builder,
            Action<VaultKeyValueEngineConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}