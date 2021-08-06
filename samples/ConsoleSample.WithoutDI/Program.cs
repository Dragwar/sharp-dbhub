using SharpDbHub;
using System;

namespace ConsoleSample.WithoutDI
{
	public class Program
	{
		private static void Main(string[] args)
		{
			const string API_KEY = "<YOUR_API_KEY>";
			using var dbHubClient = new DbHubClient(API_KEY);
			var myDatabases = dbHubClient.GetDatabases();
			Console.WriteLine($"Found the following databases: '{string.Join("', '", myDatabases)}'");
		}
	}
}
