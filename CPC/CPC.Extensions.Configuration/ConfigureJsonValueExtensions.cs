using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace CPC
{
    public static class ConfigureJsonValueExtensions
    {
        private static readonly Func<FileConfigurationProvider> ProviderCreator = () => new JsonConfigurationProvider(new JsonConfigurationSource { Optional = true });

        public static IServiceCollection ConfigureJsonValue<TOptions>(this IServiceCollection services, IConfigurationSection section)
            where TOptions : class =>
            services.ConfigureValue<TOptions>(Options.DefaultName, section, ProviderCreator);

        public static IServiceCollection ConfigureJsonValue<TOptions>(this IServiceCollection services, string name, IConfigurationSection section)
            where TOptions : class =>
            services.ConfigureValue<TOptions>(name, section, ProviderCreator);

        public static IServiceCollection ConfigureJsonValue<TOptions>(this IServiceCollection services, string value)
            where TOptions : class =>
            services.ConfigureValue<TOptions>(Options.DefaultName, value, ProviderCreator);

        public static IServiceCollection ConfigureJsonValue<TOptions>(this IServiceCollection services, string name, string value)
            where TOptions : class =>
            services.ConfigureValue<TOptions>(name, value, ProviderCreator);

        public static IServiceCollection ConfigureJsonValues<TOptions>(this IServiceCollection services, IConfiguration configuration)
            where TOptions : class =>
            services.ConfigureValues<TOptions>(configuration, ProviderCreator);
    }
}
