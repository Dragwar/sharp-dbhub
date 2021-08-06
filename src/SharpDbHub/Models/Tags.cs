using System;

namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record TagsRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

	/// <param name="Commit">The commit ID the tag corresponds to</param>
	/// <param name="Date">When the tag was created</param>
	/// <param name="Description">A free form text description of the tag</param>
	/// <param name="Email">The email address of the person who created the tag</param>
	/// <param name="Name">The name of the person who created the tag</param>
	/// <param name="Size">The file size of the tagged database, in bytes</param>
	public record TagInfo(string Commit, DateTimeOffset Date, string Description, string Email, string Name, int Size);
}
