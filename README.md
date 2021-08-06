# sharp-dbhub

A simple C# wrapper for [dbhub.io](https://dbhub.io/)'s APIs
- API documentation https://api.dbhub.io/

## NuGet
[![NuGet Badge](https://buildstats.info/nuget/SharpDbHub)](https://www.nuget.org/packages/SharpDbHub) 

## Simple creation and usage
```C#
// Simple client creation
using var dbHubClient = new DbHubClient("<YOUR_API_KEY>");

// Supports both async and sync methods 
var myDatabasesSync = dbHubClient.GetDatabases();
var myDatabasesAsync = await dbHubClient.GetDatabasesAsync(cancellationToken: cancellationToken);

Console.WriteLine($"Found the following databases: '{string.Join("', '", myDatabases)}'");
```

## Add `IDbHubClient` to dependency injection (DI) container
```C#
////---------- add DbHubClient to services ----------////
////---------- (you need to set the ApiKey before you make an api call) ----------////
//services.AddDbHubClient();

////---------- add DbHubClient to services with an ApiKey ----------////
services.AddDbHubClient(ctx.Configuration["DbHub:ApiKey"]);

////---------- add DbHubClient to services with more options ----------////
//services.AddDbHubClient(new DbHubClientOptions(new Uri("some-new-base-url"), "some-api-key", TimeSpan.FromSeconds(15)));
```

### Closing notes
I'm open to suggestions, please open a GitHub issue with your suggestion(s).

This project was built using the latest and greatest "**.net6**".
