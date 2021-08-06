using System;
using System.Collections.Generic;

namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record CommitsRequest(string DbOwner, string DbName) : BaseDbOwnerAndDbNameRequest(DbOwner, DbName);

	public record CommitsResponse(IDictionary<string, CommitInfo> CommitDetails);

	/// <param name="AuthorEmail">The email address of the person who created this commit</param>
	/// <param name="AuthorName">The name of the person who created this commit</param>
	/// <param name="CommitterEmail">The email address of the person who added the commit to this database, if different from the author</param>
	/// <param name="CommitterName">The name of the person who added the commit to this database, if different from the author</param>
	/// <param name="Id">The unique ID of this commit</param>
	/// <param name="Message">A free form text description for this commit</param>
	/// <param name="OtherParents">A list of other parent commit IDs, as used in merge commits</param>
	/// <param name="Parent">The ID of the parent commit</param>
	/// <param name="Timestamp">When this commit was created</param>
	/// <param name="Tree">Contains the information of the filesystem tree for this commit</param>
	public record CommitInfo(
		string AuthorEmail,
		string AuthorName,
		string CommitterEmail,
		string CommitterName,
		string Id,
		string Message,
		IEnumerable<string> OtherParents,
		string Parent,
		DateTimeOffset Timestamp,
		TreeInfo Tree);

	/// <param name="Id">A unique id for this tree entry</param>
	/// <param name="Entries">And array of tree entries, with one entry per filesystem object in the commit</param>
	public record TreeInfo(string Id, IEnumerable<EntryInfo> Entries);

	/// <param name="EntryType">The type of entry. Only "db" so far.</param>
	/// <param name="LastModified">When this tree entry was created</param>
	/// <param name="Licence">The sha256 (a unique identifier) of the licence for the database (up to this commit)</param>
	/// <param name="Name">The name of the database file</param>
	/// <param name="Sha256">The sha256 of the database file</param>
	/// <param name="Size">The size of the database file in bytes</param>
	public record EntryInfo(string EntryType, DateTimeOffset LastModified, string Licence, string Name, string Sha256, int Size);
}
