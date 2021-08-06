namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record TablesRequest(string DbOwner, string DbName) : BaseDbOwnerAndDbNameRequest(DbOwner, DbName);
}
