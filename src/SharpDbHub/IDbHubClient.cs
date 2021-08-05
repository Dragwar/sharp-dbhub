using SharpDbHub.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDbHub
{
	public interface IDbHubClient : IDisposable, IAsyncDisposable
	{
		IDbHubClient SetApiKey(string apiKey);

		ValueTask<IEnumerable<string>?> GetDatabasesAsync(DatabasesRequest? request = null, CancellationToken cancellationToken = default);
		IEnumerable<string>? GetDatabases(DatabasesRequest? request = null, CancellationToken cancellationToken = default);

		ValueTask<IEnumerable<string>?> GetTablesAsync(TablesRequest request, CancellationToken cancellationToken = default);
		IEnumerable<string>? GetTables(TablesRequest request, CancellationToken cancellationToken = default);

		ValueTask<IEnumerable<string>?> GetViewsAsync(ViewsRequest request, CancellationToken cancellationToken = default);
		IEnumerable<string>? GetViews(ViewsRequest request, CancellationToken cancellationToken = default);

		ValueTask<IEnumerable<IndexInfo>?> GetIndexesAsync(IndexesRequest request, CancellationToken cancellationToken = default);
		IEnumerable<IndexInfo>? GetIndexes(IndexesRequest request, CancellationToken cancellationToken = default);

		ValueTask<IEnumerable<IEnumerable<QueryResult>>?> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default);
		IEnumerable<IEnumerable<QueryResult>>? Query(QueryRequest request, CancellationToken cancellationToken = default);

		ValueTask<DeleteResponse?> DeleteAsync(DeleteRequest? request = null, CancellationToken cancellationToken = default);
		DeleteResponse? Delete(DeleteRequest? request = null, CancellationToken cancellationToken = default);

		ValueTask<UploadResponse?> UploadAsync(UploadRequest request, CancellationToken cancellationToken = default);
		UploadResponse? Upload(UploadRequest request, CancellationToken cancellationToken = default);

		ValueTask<Stream?> DownloadAsync(DownloadRequest request, CancellationToken cancellationToken = default);
		Stream? Download(DownloadRequest request, CancellationToken cancellationToken = default);

		ValueTask<byte[]?> DownloadBytesAsync(DownloadRequest request, CancellationToken cancellationToken = default);
		byte[]? DownloadBytes(DownloadRequest request, CancellationToken cancellationToken = default);

		ValueTask<WebpageResponse?> GetWebpageAsync(WebpageRequest request, CancellationToken cancellationToken = default);
		WebpageResponse? GetWebpage(WebpageRequest request, CancellationToken cancellationToken = default);
	}
}
