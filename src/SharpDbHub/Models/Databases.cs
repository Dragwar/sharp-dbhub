namespace SharpDbHub.Models
{
	/// <inheritdoc />
	public record DatabasesRequest() : BaseAuthRequest()
	{
		/// <summary>
		/// A boolean to switch between returning the list of standard databases and the list of live databases
		/// <para />
		/// NOTE: This parameter is experimental and may change
		/// </summary>
		/// <remarks>
		/// "Live" databases do not have version control. Please see more here:
		/// <see href="https://sqlitebrowser.org/blog/live-databases-are-now-live/"/>
		/// </remarks>
		public bool? Live { get; init; }
	}
}
