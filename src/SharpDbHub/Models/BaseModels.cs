namespace SharpDbHub.Models
{
	/// <param name="ApiKey">Your API key. These can be generated in your Settings page, when logged in</param>
	public abstract record BaseAuthRequest(string? ApiKey = null);

	/// <param name="DbOwner">The owner of the database</param>
	/// <param name="DbName">The name of the database</param>
	public abstract record BaseDbOwnerAndDbNameRequest(string DbOwner, string DbName) : BaseAuthRequest();
}
