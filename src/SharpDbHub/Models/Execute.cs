namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record ExecuteRequest(string DbOwner, string DbName, string Sql) : BaseDbOwnerAndDbNameRequest(DbOwner, DbName);

	public record ExecuteResponse(string Status, int RowsChanged);
}
