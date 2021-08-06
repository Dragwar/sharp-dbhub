using System;

namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record ReleasesRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

	/// <param name="Commit">The commit ID the release corresponds to</param>
	/// <param name="Date">When the release was created</param>
	/// <param name="Description">A free form text description of the release</param>
	/// <param name="Email">The email address of the person who created the release</param>
	/// <param name="Name">The name of the person who created the release</param>
	/// <param name="Size">The file size of the release database, in bytes</param>
	public record ReleaseInfo(string Commit, DateTimeOffset Date, string Description, string Email, string Name, int Size);
}
