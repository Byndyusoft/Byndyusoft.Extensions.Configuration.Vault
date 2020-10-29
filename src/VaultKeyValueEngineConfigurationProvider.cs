namespace Byndyusoft.Extensions.Configuration.Vault
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Engines;
    using Extensions;
    using Json;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;
    using Utils;

    public class VaultKeyValueEngineConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly IVaultKeyValueEngine _engine;
        private readonly object _lock = new object();
        private readonly VaultKeyValueEngineConfigurationSource _source;
        private Timer _timer;

        /// <summary>
        ///     Initializes a new <see cref="VaultKeyValueEngineConfigurationProvider" />
        /// </summary>
        /// <param name="engine">KeyValue engine instance.</param>
        /// <param name="source"></param>
        public VaultKeyValueEngineConfigurationProvider(IVaultKeyValueEngine engine, VaultKeyValueEngineConfigurationSource source)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        void IDisposable.Dispose()
        {
            _timer?.Dispose();
            _timer = null;
        }

        /// <inheritdoc />
        public override void Load()
        {
            Load(false);

            if (_source.ReloadOnChange) _timer = new Timer(_ => Load(true), null, _source.ReloadDelay, _source.ReloadDelay);
        }

        private void Load(bool reload)
        {
            if (!Monitor.TryEnter(_lock))
                return;

            try
            {
                DoLoad(reload);
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        private void DoLoad(bool reload)
        {
            var oldData = Data;

            if (reload) Data = new Dictionary<string, string>();

            var secrets = AsyncHelper.RunSync(async () => await _engine.ReadSecretsAsync());
            foreach (var secret in secrets) AddSecret(secret);

            if (reload)
            {
                var changed = DictionaryEqualityComparer<string, string>.Instance.Equals(oldData, Data) == false;
                if (changed) OnReload();
            }
        }

        private void AddSecret(VaultKeyValueSecret secret)
        {
            foreach (var pair in secret.Values)
                if (pair.Value is JObject json)
                {
                    var jsonData = new JsonObjectParser().Parse(json);
                    foreach (var jsonPair in jsonData)
                    {
                        var key = ConfigurationPath.Combine(secret.Key, pair.Key, jsonPair.Key);
                        Data.Add(key, jsonPair.Value);
                    }
                }
                else
                {
                    var key = ConfigurationPath.Combine(secret.Key, pair.Key);
                    Data.Add(key, Convert.ToString(pair.Value));
                }
        }
    }
}