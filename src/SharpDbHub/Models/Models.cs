using System.Collections.Generic;

namespace SharpDbHub.Models
{
    /// <param name="ApiKey">Your API key. These can be generated in your Settings page, when logged in</param>
    public abstract record AuthRequestBase(string? ApiKey = null);

    /// <param name="DbOwner">The owner of the database</param>
    /// <param name="DbName">The name of the database</param>
    public abstract record DbOwnerAndDbNameRequestBase(string DbOwner, string DbName) : AuthRequestBase();


    /// <inheritdoc />
    public record WebpageRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

    /// <param name="WebPage">The URL to the database in the website</param>
    public record WebpageResponse(string WebPage);


    /// <inheritdoc />
    public record ViewsRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

    /// <inheritdoc />
    public record TablesRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);


    /// <inheritdoc />
    public record DatabasesRequest() : AuthRequestBase();

    /// <inheritdoc />
    public record DownloadRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);


    /// <param name="DbName">The name of the database</param>
    public record DeleteRequest(string DbName) : AuthRequestBase();

    /// <param name="Status">The name of the database</param>
    public record DeleteResponse(string Status);


    /// <inheritdoc />
    public record IndexesRequest(string DbOwner, string DbName) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

    /// <param name="Name">The name of the index</param>
    /// <param name="Table">The name of the table the index belongs to</param>
    /// <param name="Columns">An array of objects listing the columns in the index</param>
    public record IndexInfo(string Name, string Table, IEnumerable<ColumnInfo> Columns);

    /// <param name="Id">The numeric position of the column in the index, with the first column starting at 0 </param>
    /// <param name="Name">The name of the column in the table</param>
    public record ColumnInfo(int Id, string Name);


    /// <inheritdoc cref="DbOwnerAndDbNameRequestBase" />
    /// <param name="Sql">The SQL query</param>
    public record QueryRequest(string DbOwner, string DbName, string Sql) : DbOwnerAndDbNameRequestBase(DbOwner, DbName);

    /// <param name="Name">The name of the return field</param>
    /// <param name="Type">The type of data in the field (numeric)</param>
    /// <param name="Value">The value of the field</param>
    public record QueryResult(string Name, int Type, string Value);
}
