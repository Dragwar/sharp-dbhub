using System.Text.Json;

namespace SharpDbHub
{
	internal class AllLowerCaseNamingPolicy : JsonNamingPolicy
	{
		public override string ConvertName(string name)
			=> name.Replace("_", "").ToLower();
	}
}
