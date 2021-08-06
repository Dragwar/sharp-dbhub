using System.Collections.Generic;

namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record MetadataRequest(string DbOwner, string DbName) : BaseDbOwnerAndDbNameRequest(DbOwner, DbName);

	public record MetadataResponse(
		string DefaultBranch,
		IDictionary<string, BranchInfo> Branches,
		IDictionary<string, CommitInfo> Commits,
		IDictionary<string, TagInfo> Tags,
		IDictionary<string, TagInfo> Releases,
		string WebPage);
}
