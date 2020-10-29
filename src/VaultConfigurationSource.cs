using Microsoft.Extensions.Configuration;
using System;
using Byndyusoft.Extensions.Configuration.Vault.Engines;
using VaultSharp;
using VaultSharp.V1.AuthMethods;

namespace Byndyusoft.Extensions.Configuration.Vault
{
    /// <summary>
    /// Represents a Vault as an <see cref="IConfigurationSource" />.
    /// </summary>
    public class VaultConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// The Vault Server Uri with port.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The auth method to be used to acquire a vault token.
        /// </summary>
        public IAuthMethodInfo AuthMethod { get; set; }

        /// <summary>
        ///  The version of the KeyValue secrets engine.
        /// </summary>
        public KeyValueSecretsEngine Engine { get; } = new KeyValueSecretsEngine();

        /// <summary>
        /// The Vault Api timeout.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Determines whether the source will be loaded if the underlying engine changes.
        /// </summary>
        public bool ReloadOnChange { get; set; } = false;

        /// <summary>
        /// Time interval that reload will wait before calling Load. Default is one minute.
        /// </summary>
        public TimeSpan ReloadDelay { get; set; } = TimeSpan.FromMinutes(1);

        internal VaultClientSettings AsVaultClientSettings()
        {
            return new VaultClientSettings(Uri, AuthMethod)
            {
                Namespace = Engine.Namespace,
                VaultServiceTimeout = Timeout,
                ContinueAsyncTasksOnCapturedContext = false
            };
        }

        /// <summary>
        /// Determines if loading the Vault is optional.
        /// </summary>
        public bool Optional { get; set; } = true;

        /// <summary>
        /// Builds the <see cref="VaultConfigurationProvider" /> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" />.</param>
        /// <returns>A <see cref="VaultConfigurationProvider" /></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var engine = new VaultKeyValueEngineFactory().CreateEngine(this);
            return new VaultConfigurationProvider(engine, this);
        }

        /// <summary>
        /// KeyValue secrets engine.
        /// </summary>
        public class KeyValueSecretsEngine
        {
            /// <summary>
            /// The namespace to use to achieve tenant level isolation.
            /// Enterprise Vault only.
            /// </summary>
            public string Namespace { get; set; }

            /// <summary>
            /// The name of the KeyValue secrets engine.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///  The version of the KeyValue secrets engine.
            /// </summary>
            public VaultEngineVersion Version { get; set; } = VaultEngineVersion.V2;
        }
    }
}
