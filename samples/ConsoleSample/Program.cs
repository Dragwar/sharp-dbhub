using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpDbHub;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSample
{
	public class Program : IHostedService
	{
		private readonly ILogger<Program> _logger;
		private readonly IDbHubClient _dbHubClient;

		public Program(ILogger<Program> logger, IDbHubClient dbHubClient)
		{
			_logger = logger;
			_dbHubClient = dbHubClient;
		}

		private async static Task Main(string[] args)
			=> await Host.CreateDefaultBuilder(args)
			.ConfigureHostConfiguration(ConfigureHost)
			.ConfigureServices(ConfigureServices)
			.RunConsoleAsync(CancellationToken.None);

		private static void ConfigureHost(IConfigurationBuilder builder)
		{
			builder.AddEnvironmentVariables();
			builder.AddUserSecrets(Assembly.GetExecutingAssembly());
		}

		private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
		{
			services.AddHostedService<Program>();

			// add DbHubClient to services
			// (you need to set the ApiKey before you make an api call - see how in StartAsync method)
			//services.AddDbHubClient();

			// add DbHubClient to services with an ApiKey
			services.AddDbHubClient(ctx.Configuration["DbHub:ApiKey"]);

			// add DbHubClient to services with more options
			//services.AddDbHubClient(new DbHubClientOptions(new Uri("some-new-base-url"), "some-api-key", TimeSpan.FromSeconds(15)));

		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			// async call
			var dbs1 = await _dbHubClient.GetDatabasesAsync(cancellationToken: cancellationToken);
			_logger.LogInformation("(async) Found {0} dbs", dbs1.Count());
			_logger.LogInformation(string.Join(", ", dbs1));

			// sync call
			var dbs2 = _dbHubClient.GetDatabases(cancellationToken: cancellationToken);
			_logger.LogInformation("(sync) Found {0} dbs", dbs2.Count());
			_logger.LogInformation(string.Join(", ", dbs2));

			// if you want to change the ApiKey (stored for future requests)
			//_dbHubClient.SetApiKey("<some-api-key>");

			// or pass the ApiKey along with the request object (not stored for future requests)
			//_dbHubClient.GetDatabases(new() { ApiKey = "<some-api-key>" }, cancellationToken: cancellationToken);

			// Upload/UploadAsync will dispose the stream
			//var uploadResponse = _dbHubClient.Upload(new UploadRequest(File.OpenRead("<SQL_LITE_DB_FILE_PATH>"), "someUserName", "test.db"), cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _dbHubClient.DisposeAsync();

			// sync dispose if you prefer
			//_dbHubClient.Dispose();
		}
	}
}
