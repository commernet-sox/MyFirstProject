using AspectCore.DependencyInjection;
using Com.Ctrip.Framework.Apollo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CPC
{
    public static class ConfigurationExtensions
    {
        public static T Bind<T>(this IConfiguration configuration, string key = "")
    where T : class, new()
        {
            if (key.IsNull())
            {
                key = typeof(T).Name;
            }
            var data = new T();
            configuration.Bind(key, data);
            return data;
        }

        public static IServiceContext Configure<TOptions>(this IServiceContext services, Action<TOptions> configureOptions)
            where TOptions : class
        => services.Configure(Options.DefaultName, configureOptions);

        public static IServiceContext Configure<TOptions>(this IServiceContext services, string name, Action<TOptions> configureOptions)
    where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddOptions();
            services.AddInstance<IConfigureOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(name, configureOptions));
            return services;
        }

        public static IServiceContext Configure<TOptions>(this IServiceContext services, IConfiguration config)
where TOptions : class => services.Configure<TOptions>(Options.DefaultName, config);

        public static IServiceContext Configure<TOptions>(this IServiceContext services, string name, IConfiguration config)
where TOptions : class => services.Configure<TOptions>(name, config, _ => { });

        public static IServiceContext Configure<TOptions>(this IServiceContext services, IConfiguration config, Action<BinderOptions> configureBinder)
where TOptions : class => services.Configure<TOptions>(Options.DefaultName, config, configureBinder);

        public static IServiceContext Configure<TOptions>(this IServiceContext services, string name, IConfiguration config, Action<BinderOptions> configureBinder)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddOptions();
            services.AddInstance<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            return services.AddInstance<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, config, configureBinder));
        }

        public static IServiceContext AddOptions(this IServiceContext services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddType(typeof(IOptions<>), typeof(OptionsManager<>), Lifetime.Singleton);
            services.TryAddType(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>), Lifetime.Scoped);
            services.TryAddType(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>), Lifetime.Singleton);
            services.TryAddType(typeof(IOptionsFactory<>), typeof(OptionsFactory<>), Lifetime.Transient);
            services.TryAddType(typeof(IOptionsMonitorCache<>), typeof(OptionsCache<>), Lifetime.Singleton);
            return services;
        }

        public static IConfigurationBuilder AddApollo(this IConfigurationBuilder builder, bool post = true, string key = "Apollo")
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var config = builder.Build();
            var apolloSection = config.GetSection(key);
            if (!apolloSection.Exists())
            {
                return builder;
            }

            if (post)
            {
                return new ConfigurationBuilder().AddApollo(apolloSection).AddDefault().AddConfiguration(config);
            }
            return builder.AddApollo(apolloSection).AddDefault();
        }

        private static readonly ConcurrentDictionary<IConfiguration, List<ConfigurationSectionData>> _configurationPools = new ConcurrentDictionary<IConfiguration, List<ConfigurationSectionData>>();

        public static void Change(this IConfiguration configuration, string key, Action<ConfigurationSectionData> change)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (change == null)
            {
                throw new ArgumentNullException(nameof(change));
            }

            if (!_configurationPools.TryGetValue(configuration, out var list))
            {
                list = new List<ConfigurationSectionData>();

                ChangeToken.OnChange(() => configuration.GetReloadToken(), () =>
                {
                    if (_configurationPools.TryGetValue(configuration, out var tmpList))
                    {
                        foreach (var item in tmpList)
                        {
                            item.ChangeValue(configuration[item.Key]);
                        }
                    }
                });
            }

            list.Add(new ConfigurationSectionData(key, configuration[key], change));
            _configurationPools.AddOrUpdate(configuration, list, (k, v) => list);
        }
    }

    public class ConfigurationSectionData
    {
        private readonly Action<ConfigurationSectionData> eventChange;

        public string Key { get; private set; }

        public string OriginalValue { get; private set; }

        public string CurrentValue { get; private set; }

        public ConfigurationSectionData(string key, string originalValue, Action<ConfigurationSectionData> change)
        {
            Key = key;
            OriginalValue = originalValue;
            CurrentValue = originalValue;
            eventChange = change;
        }

        public void ChangeValue(string value)
        {
            if (value != CurrentValue)
            {
                OriginalValue = CurrentValue;
                CurrentValue = value;
                eventChange?.Invoke(this);
            }
        }
    }
}
