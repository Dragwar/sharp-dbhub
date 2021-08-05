using System;

namespace SharpDbHub
{
	public record DbHubClientOptions
	{
		public const string DEFAULT_BASE_URL = "https://api.dbhub.io/v1/";
		private static readonly Uri _defaultBaseUrl = new(DEFAULT_BASE_URL);

		public DbHubClientOptions(Uri baseUrl, string apiKey, TimeSpan? timeout = null)
		{
			BaseUrl = baseUrl;
			ApiKey = apiKey;
			Timeout = timeout;
		}

		public DbHubClientOptions(string apiKey, TimeSpan? timeout = null)
			: this(_defaultBaseUrl, apiKey, timeout)
		{
		}

		public DbHubClientOptions(TimeSpan? timeout = null)
			: this(_defaultBaseUrl, "", timeout)
		{
		}

		public DbHubClientOptions()
		{
		}


		public static DbHubClientOptions Default => new(_defaultBaseUrl, "");

		public bool HasApiKey => !string.IsNullOrWhiteSpace(ApiKey);

		public Uri? BaseUrl { get; init; }
		public string? ApiKey { get; init; }
		public TimeSpan? Timeout { get; init; }
	}
}
