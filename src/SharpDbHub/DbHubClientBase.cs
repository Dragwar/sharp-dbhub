using SharpDbHub.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDbHub
{
	public abstract class DbHubClientBase : IDisposable, IAsyncDisposable
	{
		protected static readonly Lazy<HttpClient> _fallbackHttpClient = new(() => new HttpClient());
		protected readonly JsonSerializerOptions _jsonSerializeOptions = new() { PropertyNamingPolicy = new AllLowerCaseNamingPolicy() };
		protected readonly JsonSerializerOptions _jsonDeserializeOptions = new() { PropertyNamingPolicy = new AllLowerCaseSnakeCaseNamingPolicy() };
		protected readonly JsonSerializerOptions _jsonDeserializeQueryOptions = new() { PropertyNameCaseInsensitive = true };

		protected readonly HttpClient _httpClient;

		protected DbHubClientOptions Options { get; set; }

		protected DbHubClientBase(HttpClient httpClient, DbHubClientOptions? options = null)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			Options = options ?? DbHubClientOptions.Default;
			if (Options.BaseUrl is not null && Options.BaseUrl.ToString() != httpClient.BaseAddress?.ToString())
			{
				httpClient.BaseAddress = Options.BaseUrl;
			}
			if (Options.Timeout is not null && Options.Timeout != httpClient.Timeout)
			{
				httpClient.Timeout = Options.Timeout.Value;
			}
		}

		#region Dispose Methods
		private bool _isDisposed;
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					_httpClient?.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_isDisposed = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual ValueTask DisposeAsync(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					_httpClient?.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_isDisposed = true;
			}
			return ValueTask.CompletedTask;
		}

		public async ValueTask DisposeAsync()
		{
			// Do not change this code. Put cleanup code in 'DisposeAsync(bool disposing)' method
			await DisposeAsync(disposing: true);
			GC.SuppressFinalize(this);
		}


		// // TODO: override finalizer only if 'Dispose(bool disposing)' and 'DisposeAsync(bool disposing)' has code to free unmanaged resources
		// ~DbHubClientBase()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' and 'DisposeAsync(bool disposing)' method
		//     Dispose(disposing: false);
		// }
		#endregion

		protected FormUrlEncodedContent CreateContent<T>(T? request) where T : BaseAuthRequest
		{
			request = TrySetApiFallback(request);
			IEnumerable<KeyValuePair<string, string>>? keyValuePairs = null;
			if (request is not null)
			{
				var json = JsonSerializer.Serialize(request, options: _jsonSerializeOptions);
				using var jDoc = JsonDocument.Parse(json);
				var enumerable = jDoc.RootElement.EnumerateObject().ToArray();
				keyValuePairs = enumerable.Select(CreateKeyValuePair()).ToArray();
			}
			keyValuePairs ??= new[] { KeyValuePair.Create("apikey", Options.ApiKey) }!;
			keyValuePairs = keyValuePairs.Where(x => !string.IsNullOrWhiteSpace(x.Value));
			var content = new FormUrlEncodedContent(keyValuePairs!);
			return content;
		}

		protected async Task<FormUrlEncodedContent> CreateContentAsync<T>(T? request, CancellationToken cancellationToken)
			where T : BaseAuthRequest
		{
			request = TrySetApiFallback(request);
			IEnumerable<KeyValuePair<string, string>>? keyValuePairs = null;
			if (request is not null)
			{
				using var ms = new MemoryStream();
				await JsonSerializer.SerializeAsync(ms, request, _jsonSerializeOptions, cancellationToken);
				var json = Encoding.UTF8.GetString(ms.ToArray());
				using var utf8JsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
				using var jDoc = await JsonDocument.ParseAsync(utf8JsonStream, cancellationToken: cancellationToken);
				var enumerable = jDoc.RootElement.EnumerateObject();
				keyValuePairs = enumerable.Select(CreateKeyValuePair()).ToArray();
			}
			keyValuePairs ??= new[] { KeyValuePair.Create("apikey", Options.ApiKey) }!;
			keyValuePairs = keyValuePairs.Where(x => !string.IsNullOrWhiteSpace(x.Value));
			var content = new FormUrlEncodedContent(keyValuePairs!);
			return content;
		}

		private static Func<JsonProperty, KeyValuePair<string, string>> CreateKeyValuePair() => jp => KeyValuePair.Create(jp.Name, jp.Value.ToString());

		protected T? TrySetApiFallback<T>(T? request)
			where T : BaseAuthRequest => request != null
			? request with
			{
				ApiKey = !string.IsNullOrWhiteSpace(request.ApiKey)
					? request.ApiKey
					: Options.ApiKey
			}
			: null;

		#region Send Request Methods
		protected virtual HttpResponseMessage SendRequest<T>(string endpoint, T? request, CancellationToken cancellationToken = default)
			where T : BaseAuthRequest
		{
			request = TrySetApiFallback(request);
			FormUrlEncodedContent content = CreateContent(request);

			var syncHttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
			{
				Content = content
			};
			var response = _httpClient.Send(syncHttpRequestMessage, cancellationToken);
			response.EnsureSuccessStatusCode();
			return response;
		}

		protected virtual async ValueTask<HttpResponseMessage> SendRequestAsync<T>(string endpoint, T? request, CancellationToken cancellationToken = default)
			where T : BaseAuthRequest
		{
			request = TrySetApiFallback(request);
			FormUrlEncodedContent content = await CreateContentAsync(request, cancellationToken);

			var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
			response.EnsureSuccessStatusCode();
			return response;
		}
		#endregion

		#region Read Content As Methods
		protected virtual T? ReadContentAs<T>(HttpContent content, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
		{
			using var cs = content.ReadAsStream(cancellationToken);
			using var sr = new StreamReader(cs);
			return JsonSerializer.Deserialize<T>(sr.ReadToEnd(), jsonSerializerOptions);
		}

		protected virtual T? ReadContentAs<T>(HttpContent content, CancellationToken cancellationToken = default)
			=> ReadContentAs<T>(content, _jsonDeserializeOptions, cancellationToken);

		protected virtual async ValueTask<T?> ReadContentAsAsync<T>(HttpContent content, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
			=> await content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken);

		protected virtual async ValueTask<T?> ReadContentAsAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
			=> await ReadContentAsAsync<T>(content, _jsonDeserializeOptions, cancellationToken);
		#endregion
	}
}
