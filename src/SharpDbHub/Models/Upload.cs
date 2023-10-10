using System.IO;
using System.Text.Json.Serialization;

namespace SharpDbHub.Models
{
	public record UploadRequest : BaseDbOwnerAndDbNameRequest
	{
		/// <param name="dbOwner">The owner of the database</param>
		/// <param name="dbName">Optional (but recommended) - The name of the database</param>
		/// <param name="commit">Only required for existing databases - The commit ID which this new upload should be appended to</param>
		public UploadRequest(Stream file, string dbOwner, string? dbName = "", string? commit = null)
			: base(dbOwner, dbName!)
		{
			DbOwner = dbOwner;
			DbName = dbName!;
			Commit = commit;
			File = file;
		}

		/// <summary>
		/// The commit ID which this new upload should be appended to
		/// </summary>
		public string? Commit { get; init; }

		/// <summary>
		/// The email address of the person who created this commit
		/// </summary>
		public string? AuthorEmail { get; init; }

		/// <summary>
		/// The name of the person who created this commit
		/// </summary>
		public string? AuthorName { get; init; }

		/// <summary>
		/// The name of the branch this database will be uploaded to
		/// </summary>
		public string? Branch { get; init; }

		/// <summary>
		/// A free form text description for this commit
		/// </summary>
		public string? CommitMsg { get; init; }

		/// <summary>
		/// The email address of the person who added the commit to this database, if different from the author 
		/// </summary>
		public string? CommitterEmail { get; init; }

		/// <summary>
		/// The name of the person who added the commit to this database, if different from the author 
		/// </summary>
		public string? CommitterName { get; init; }

		/// <summary>
		/// This becomes the upload commit's creation timestamp. A text string in RFC 3339 format. 
		/// </summary>
		public string? CommitTimeStamp { get; init; }

		/// <summary>
		/// The SHA256 checksum of the database file being uploaded. 
		/// If you include this, the DBHub.io cloud will verify that the uploaded database file matches this checksum. 
		/// </summary>
		public string? Dbshasum { get; init; }

		/// <summary>
		/// Only useful when uploading a database over the top of existing commits in a branch. 
		/// Without this option being set (to true), the DBHub.io cloud will reject the upload as it could result in data loss. 
		/// Don't use this option unless you understand in depth what it does 
		/// </summary>
		public bool? Force { get; init; }

		/// <summary>
		/// This value will be used for the "Last Modified" field in the new commit of the database. A text string in RFC 3339 format. 
		/// </summary>
		public string? LastModified { get; init; }

		/// <summary>
		/// The sha256 (a unique identifier) of the licence for the database (up to this commit)
		/// </summary>
		public string? Licence { get; init; }

		/// <summary>
		/// A list of other parent commit IDs, as used in merge commits
		/// </summary>
		public string? OtherParents { get; init; }

		/// <summary>
		/// Should this database be public (true) or private (false)?
		/// </summary>
		public bool? Public { get; init; }

		/// <summary>
		/// The URL to the reference source of the data
		/// </summary>
		public string? SourceUrl { get; init; }

		[JsonIgnore]
		public Stream File { get; init; }

		/// <summary>
		/// A boolean indicating whether this upload is a live database
		/// <para />
		/// NOTE: This parameter is experimental and may change
		/// </summary>
		/// <remarks>
		/// "Live" databases do not have version control. Please see more here:
		/// <see href="https://sqlitebrowser.org/blog/live-databases-are-now-live/"/>
		/// </remarks>
		public bool? Live { get; init; }
	}

	/// <param name="Commit">The unique ID of the new commit</param>
	/// <param name="Url">The web browser URL of the new database, or database revision</param>
	public record UploadResponse(string Commit, string Url);
}
