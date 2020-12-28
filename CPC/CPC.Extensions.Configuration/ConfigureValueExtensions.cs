using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CPC
{
    public static class ConfigureValueExtensions
    {
        public static IServiceCollection ConfigureValue<TOptions>(this IServiceCollection services, string name, IConfigurationSection section, Func<FileConfigurationProvider> creator)
            where TOptions : class
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            return services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, section))
                .Configure<TOptions>(name, options => creator().Bind(options, section.Value));
        }

        public static IServiceCollection ConfigureValue<TOptions>(this IServiceCollection services, IConfigurationSection section, Func<FileConfigurationProvider> creator)
            where TOptions : class
            => services.ConfigureValue<TOptions>(Options.DefaultName, section, creator);

        public static IServiceCollection ConfigureValue<TOptions>(this IServiceCollection services, string name, string value, Func<FileConfigurationProvider> creator)
    where TOptions : class => services.Configure<TOptions>(name, options => creator().Bind(options, value));

        public static IServiceCollection ConfigureValue<TOptions>(this IServiceCollection services, string value, Func<FileConfigurationProvider> creator)
   where TOptions : class => services.ConfigureValue<TOptions>(Options.DefaultName, value, creator);

        public static void Bind<TOptions>(this FileConfigurationProvider provider, TOptions options, string value)
            where TOptions : class
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var root = new ConfigurationRoot(new List<IConfigurationProvider> { provider });

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                provider.Load(stream);
            }

            root.Bind(options);
        }

        public static IServiceCollection ConfigureValues<TOptions>(this IServiceCollection services, IConfiguration configuration, Func<FileConfigurationProvider> creator)
            where TOptions : class
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            return services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(configuration))
                .AddOptions()
                .AddSingleton<IConfigureOptions<TOptions>>(new SectionValueConfigureOptions<TOptions>(configuration,
                    (options, value) => creator().Bind(options, value)));
        }
    }
}
