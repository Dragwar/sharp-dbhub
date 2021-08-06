using System.Collections.Generic;

namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record BranchesRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

	/// <param name="DefaultBranch">The name of the default branch for the database. eg: "master"</param>
	/// <param name="Branches">Contains details of all branches in the database, as an unordered set of "branch name": { branch details } pairs</param>
	public record BranchesResponse(string DefaultBranch, IDictionary<string, BranchInfo> Branches);

	/// <param name="Commit">The commit ID of the branch HEAD</param>
	/// <param name="CommitCount">The number of commits in the branch</param>
	/// <param name="Description">A free form text description for the branch</param>
	public record BranchInfo(string Commit, int CommitCount, string Description);
}
