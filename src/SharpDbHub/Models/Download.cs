namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record DownloadRequest(string DbOwner, string DbName) : BaseDbOwnerAndDbNameRequest(DbOwner, DbName);
}
