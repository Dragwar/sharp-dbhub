namespace SharpDbHub.Models
{
    /// <inheritdoc cref="DbOwnerAndDbNameRequestBase" />
    /// <param name="Sql">The SQL query</param>
    public record QueryRequest(string DbOwner, string DbName, string Sql) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

    /// <param name="Name">The name of the return field</param>
    /// <param name="Type">The type of data in the field (numeric)</param>
    /// <param name="Value">The value of the field</param>
    public record QueryResult(string Name, int Type, string Value);
}
