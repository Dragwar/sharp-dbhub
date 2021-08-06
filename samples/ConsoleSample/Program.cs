using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpDbHub;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSample
{
	public class Program : IHostedService
	{
		private readonly ILogger<Program> _logger;
		private readonly IHostApplicationLifetime _lifetime;
		private readonly IDbHubClient _dbHubClient;

		public Program(ILogger<Program> logger, IHostApplicationLifetime lifetime, IDbHubClient dbHubClient)
		{
			_logger = logger;
			_lifetime = lifetime;
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

			////---------- add DbHubClient to services ----------////
			////---------- (you need to set the ApiKey before you make an api call - see how in PromptIfApiKeyIsNotPresent method) ----------////
			//services.AddDbHubClient();

			////---------- add DbHubClient to services with an ApiKey ----------////
			services.AddDbHubClient(ctx.Configuration["DbHub:ApiKey"]);

			////---------- add DbHubClient to services with more options ----------////
			//services.AddDbHubClient(new DbHubClientOptions(new Uri("some-new-base-url"), "some-api-key", TimeSpan.FromSeconds(15)));
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				PromptIfApiKeyIsNotPresent();

				////---------- async call ----------////
				var dbs1 = await _dbHubClient.GetDatabasesAsync(cancellationToken: cancellationToken);
				_logger.LogInformation("(async) Found {0} dbs", dbs1.Count());
				_logger.LogInformation(string.Join(", ", dbs1));

				////---------- sync call ----------////
				var dbs2 = _dbHubClient.GetDatabases(cancellationToken: cancellationToken);
				_logger.LogInformation("(sync) Found {0} dbs", dbs2.Count());
				_logger.LogInformation(string.Join(", ", dbs2));

				////---------- if you want to change the ApiKey (stored for future requests) ----------////
				//_dbHubClient.SetApiKey("<some-api-key>");

				////---------- or pass the ApiKey along with the request object (not stored for future requests) ----------////
				//_dbHubClient.GetDatabases(new() { ApiKey = "<some-api-key>" }, cancellationToken: cancellationToken);


				////---------- Upload/UploadAsync will dispose the stream ----------////
				//var uploadResponse = _dbHubClient.Upload(new UploadRequest(File.OpenRead("<SQL_LITE_DB_FILE_PATH>"), "someUserName", "test.db"), cancellationToken);

				if (PromptShouldStopExecution())
				{
					_lifetime.StopApplication();
				}
			}

			static bool PromptShouldStopExecution()
			{
				Console.WriteLine();
				Console.Write("Execute Again? (Y/N) ");
				var answer = Console.ReadKey().KeyChar.ToString();
				Console.WriteLine();
				Console.WriteLine();
				return !"Y".Equals(answer, StringComparison.OrdinalIgnoreCase);
			}
		}

		private void PromptIfApiKeyIsNotPresent()
		{
			while (!_dbHubClient.HasApiKey())
			{
				Console.WriteLine("Please input an ApiKey");
				_dbHubClient.SetApiKey(Console.ReadLine());
			}
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			////---------- async dispose ----------////
			await _dbHubClient.DisposeAsync();

			////---------- sync dispose if you prefer ----------////
			//_dbHubClient.Dispose();
		}
	}
}
