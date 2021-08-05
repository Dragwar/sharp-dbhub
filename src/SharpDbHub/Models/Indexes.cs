using System.Collections.Generic;

namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record IndexesRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

	/// <param name="Name">The name of the index</param>
	/// <param name="Table">The name of the table the index belongs to</param>
	/// <param name="Columns">An array of objects listing the columns in the index</param>
	public record IndexInfo(string Name, string Table, IEnumerable<ColumnInfo> Columns);

	/// <param name="Id">The numeric position of the column in the index, with the first column starting at 0 </param>
	/// <param name="Name">The name of the column in the table</param>
	public record ColumnInfo(int Id, string Name);
}
