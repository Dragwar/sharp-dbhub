namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record ViewsRequest(string DbOwner, string DbName) : BaseDbOwnerAndDbNameRequest(DbOwner, DbName);
}
