﻿using SharpDbHub.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDbHub
{
	public class DbHubClient : DbHubClientBase, IDbHubClient
	{
		public DbHubClient(HttpClient httpClient, DbHubClientOptions? options = null)
			: base(httpClient, options)
		{
		}

		public DbHubClient(IHttpClientFactory httpClientFactory, DbHubClientOptions? options = null)
			: base(httpClientFactory.CreateClient(), options)
		{
		}

		public DbHubClient(string apiKey)
			: base(_fallbackHttpClient.Value, DbHubClientOptions.Default with { ApiKey = apiKey })
		{
		}

		public DbHubClient()
			: base(_fallbackHttpClient.Value)
		{
		}

		private static bool IsBase64String(string base64)
		{
			Span<byte> buffer = stackalloc byte[base64.Length];
			return Convert.TryFromBase64String(base64, buffer, out _);
		}

		public bool HasApiKey() => Options.HasApiKey;
		public IDbHubClient SetApiKey(string? apiKey)
		{
			Options = Options with { ApiKey = apiKey };
			return this;
		}

		public async ValueTask<IEnumerable<string>?> GetDatabasesAsync(DatabasesRequest? request = null, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("databases", request!, cancellationToken);
			return await ReadContentAsAsync<IEnumerable<string>>(response.Content, cancellationToken);
		}

		public IEnumerable<string>? GetDatabases(DatabasesRequest? request = null, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("databases", request!, cancellationToken);
			return ReadContentAs<IEnumerable<string>>(response.Content, cancellationToken);
		}

		public async ValueTask<IEnumerable<string>?> GetTablesAsync(TablesRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("tables", request, cancellationToken);
			return await ReadContentAsAsync<IEnumerable<string>>(response.Content, cancellationToken);
		}

		public IEnumerable<string>? GetTables(TablesRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("tables", request, cancellationToken);
			return ReadContentAs<IEnumerable<string>>(response.Content, cancellationToken);
		}

		public async ValueTask<IEnumerable<string>?> GetViewsAsync(ViewsRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("views", request, cancellationToken);
			return await ReadContentAsAsync<IEnumerable<string>>(response.Content, cancellationToken);
		}

		public IEnumerable<string>? GetViews(ViewsRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("views", request, cancellationToken);
			return ReadContentAs<IEnumerable<string>>(response.Content, cancellationToken);
		}

		public async ValueTask<IEnumerable<IndexInfo>?> GetIndexesAsync(IndexesRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("indexes", request, cancellationToken);
			return await ReadContentAsAsync<IEnumerable<IndexInfo>>(response.Content, cancellationToken);
		}

		public IEnumerable<IndexInfo>? GetIndexes(IndexesRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("indexes", request, cancellationToken);
			return ReadContentAs<IEnumerable<IndexInfo>>(response.Content, cancellationToken);
		}

		public async ValueTask<IEnumerable<IEnumerable<QueryResult>>?> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default)
		{
			if (!IsBase64String(request.Sql))
			{
				request = request with { Sql = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Sql)) };
			}
			var response = await SendRequestAsync("query", request, cancellationToken);
			return await ReadContentAsAsync<IEnumerable<IEnumerable<QueryResult>>>(response.Content, _jsonDeserializeQueryOptions, cancellationToken);
		}

		public IEnumerable<IEnumerable<QueryResult>>? Query(QueryRequest request, CancellationToken cancellationToken = default)
		{
			if (!IsBase64String(request.Sql))
			{
				request = request with { Sql = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Sql)) };
			}
			var response = SendRequest("query", request, cancellationToken);
			return ReadContentAs<IEnumerable<IEnumerable<QueryResult>>>(response.Content, _jsonDeserializeQueryOptions, cancellationToken);
		}

		public async ValueTask<DeleteResponse?> DeleteAsync(DeleteRequest? request = null, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("delete", request, cancellationToken);
			return await ReadContentAsAsync<DeleteResponse>(response.Content, cancellationToken);
		}

		public DeleteResponse? Delete(DeleteRequest? request = null, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("delete", request, cancellationToken);
			return ReadContentAs<DeleteResponse>(response.Content, cancellationToken);
		}

		public async ValueTask<UploadResponse?> UploadAsync(UploadRequest request, CancellationToken cancellationToken = default)
		{
			request = TrySetApiFallback(request)!;
			using var content = CreateContent(request);

			using var multipartContent = new MultipartFormDataContent();
			var str = await content.ReadAsStringAsync(cancellationToken);
			var allPairs = str?.Split("&", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
			foreach (var pair in allPairs)
			{
				var parts = pair.Split("=");
				multipartContent.Add(new StringContent(parts[1]), parts[0]);
			}
			if (!string.IsNullOrWhiteSpace(request.DbName))
			{
				multipartContent.Add(new StreamContent(request.File), "file", request.DbName);
			}
			else
			{
				multipartContent.Add(new StreamContent(request.File), "file");
			}

			var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "upload") { Content = multipartContent }, cancellationToken);
			return await ReadContentAsAsync<UploadResponse>(response.Content, cancellationToken);
		}

		public UploadResponse? Upload(UploadRequest request, CancellationToken cancellationToken = default)
		{
			request = TrySetApiFallback(request)!;
			using var content = CreateContent(request);

			using var multipartContent = new MultipartFormDataContent();
			using var sr = new StreamReader(content.ReadAsStream(cancellationToken));
			var allPairs = sr.ReadToEnd()?.Split("&", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
			foreach (var pair in allPairs)
			{
				var parts = pair.Split("=");
				multipartContent.Add(new StringContent(parts[1]), parts[0]);
			}
			if (!string.IsNullOrWhiteSpace(request.DbName))
			{
				multipartContent.Add(new StreamContent(request.File), "file", request.DbName);
			}
			else
			{
				multipartContent.Add(new StreamContent(request.File), "file");
			}

			var response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Post, "upload") { Content = multipartContent }, cancellationToken);
			return ReadContentAs<UploadResponse>(response.Content, cancellationToken);
		}

		public async ValueTask<Stream?> DownloadAsync(DownloadRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("download", request, cancellationToken);
			return await response.Content.ReadAsStreamAsync(cancellationToken);
		}

		public Stream? Download(DownloadRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("download", request, cancellationToken);
			return response.Content.ReadAsStream(cancellationToken);
		}

		public async ValueTask<byte[]?> DownloadBytesAsync(DownloadRequest request, CancellationToken cancellationToken = default)
		{
			await using var stream = await DownloadAsync(request, cancellationToken);
			await using var ms = new MemoryStream();
			stream?.CopyToAsync(ms, cancellationToken);
			return ms?.ToArray();
		}

		public byte[]? DownloadBytes(DownloadRequest request, CancellationToken cancellationToken = default)
		{
			using var stream = Download(request, cancellationToken);
			using var ms = new MemoryStream();
			stream?.CopyTo(ms);
			return ms?.ToArray();
		}

		public async ValueTask<WebpageResponse?> GetWebpageAsync(WebpageRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("webpage", request, cancellationToken);
			return await ReadContentAsAsync<WebpageResponse>(response.Content, cancellationToken);
		}

		public WebpageResponse? GetWebpage(WebpageRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("webpage", request, cancellationToken);
			return ReadContentAs<WebpageResponse>(response.Content, cancellationToken);
		}

		public async ValueTask<BranchesResponse?> GetBranchesAsync(BranchesRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("branches", request, cancellationToken);
			return await ReadContentAsAsync<BranchesResponse>(response.Content, cancellationToken);
		}

		public BranchesResponse? GetBranches(BranchesRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("branches", request, cancellationToken);
			return ReadContentAs<BranchesResponse>(response.Content, cancellationToken);
		}

		public async ValueTask<IDictionary<string, CommitInfo>?> GetCommitsAsync(CommitsRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("commits", request, cancellationToken);
			return await ReadContentAsAsync<IDictionary<string, CommitInfo>>(response.Content, cancellationToken);
		}

		public IDictionary<string, CommitInfo>? GetCommits(CommitsRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("commits", request, cancellationToken);
			return ReadContentAs<IDictionary<string, CommitInfo>>(response.Content, cancellationToken);
		}

		public async ValueTask<IDictionary<string, TagInfo>?> GetTagsAsync(TagsRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("tags", request, cancellationToken);
			return await ReadContentAsAsync<IDictionary<string, TagInfo>>(response.Content, cancellationToken);
		}

		public IDictionary<string, TagInfo>? GetTags(TagsRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("tags", request, cancellationToken);
			return ReadContentAs<IDictionary<string, TagInfo>>(response.Content, cancellationToken);
		}

		public async ValueTask<IDictionary<string, ReleaseInfo>?> GetReleasesAsync(ReleasesRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("releases", request, cancellationToken);
			return await ReadContentAsAsync<IDictionary<string, ReleaseInfo>>(response.Content, cancellationToken);
		}

		public IDictionary<string, ReleaseInfo>? GetReleases(ReleasesRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("releases", request, cancellationToken);
			return ReadContentAs<IDictionary<string, ReleaseInfo>>(response.Content, cancellationToken);
		}

		public async ValueTask<MetadataResponse?> GetMetadataAsync(MetadataRequest request, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync("metadata", request, cancellationToken);
			return await ReadContentAsAsync<MetadataResponse>(response.Content, cancellationToken);
		}

		public MetadataResponse? GetMetadata(MetadataRequest request, CancellationToken cancellationToken = default)
		{
			var response = SendRequest("metadata", request, cancellationToken);
			return ReadContentAs<MetadataResponse>(response.Content, cancellationToken);
		}

		public async ValueTask<ExecuteResponse?> ExecuteAsync(ExecuteRequest request, CancellationToken cancellationToken = default)
		{
			if (!IsBase64String(request.Sql))
			{
				request = request with { Sql = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Sql)) };
			}
			var response = await SendRequestAsync("execute", request, cancellationToken);
			return await ReadContentAsAsync<ExecuteResponse>(response.Content, cancellationToken);
		}

		public ExecuteResponse? Execute(ExecuteRequest request, CancellationToken cancellationToken = default)
		{
			if (!IsBase64String(request.Sql))
			{
				request = request with { Sql = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Sql)) };
			}
			var response = SendRequest("execute", request, cancellationToken);
			return ReadContentAs<ExecuteResponse>(response.Content, cancellationToken);
		}
	}
}
