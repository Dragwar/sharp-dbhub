namespace SharpDbHub.Models
{
    /// <inheritdoc />
    public record WebpageRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

    /// <param name="WebPage">The URL to the database in the website</param>
    public record WebpageResponse(string WebPage);
}
