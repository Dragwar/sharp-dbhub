namespace SharpDbHub.Models
{
    /// <param name="DbName">The name of the database</param>
    public record DeleteRequest(string DbName) : AuthRequestBase();

    /// <param name="Status">The name of the database</param>
    public record DeleteResponse(string Status);
}
