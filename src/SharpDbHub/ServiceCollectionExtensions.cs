using SharpDbHub;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDbHubClient(this IServiceCollection services)
			=> AddDbHubClient(services, DbHubClientOptions.Default);

		public static IServiceCollection AddDbHubClient(this IServiceCollection services, string apiKey)
			=> AddDbHubClient(services, DbHubClientOptions.Default with { ApiKey = apiKey });

		public static IServiceCollection AddDbHubClient(this IServiceCollection services, DbHubClientOptions options)
		{
			options ??= DbHubClientOptions.Default;
			services.AddTransient(static _ => DbHubClientOptions.Default);
			services.AddHttpClient<IDbHubClient, DbHubClient>(ConfigureHttpClient(options));
			services.AddHttpClient<DbHubClientBase, DbHubClient>(ConfigureHttpClient(options));
			return services;

			static Func<HttpClient, DbHubClient> ConfigureHttpClient(DbHubClientOptions options)
				=> http => new DbHubClient(http, options);
		}
	}
}
