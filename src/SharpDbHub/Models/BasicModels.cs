namespace SharpDbHub.Models
{
    /// <param name="ApiKey">Your API key. These can be generated in your Settings page, when logged in</param>
    public abstract record AuthRequestBase(string? ApiKey = null);

    /// <param name="DbOwner">The owner of the database</param>
    /// <param name="DbName">The name of the database</param>
    public abstract record DbOwnerAndDbNameRequestBase(string DbOwner, string DbName) : AuthRequestBase();


    /// <inheritdoc />
    public record ViewsRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

    /// <inheritdoc />
    public record TablesRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);


    /// <inheritdoc />
    public record DatabasesRequest() : AuthRequestBase();

    /// <inheritdoc />
    public record DownloadRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);
}
