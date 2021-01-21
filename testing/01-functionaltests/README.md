# Testing

## Recipe 01 - Functional tests

```cmd
dotnet new webapi -n FooApi -o src/
dotnet new nunit -n FooApiFunctionalTests -o tests/
```

 - install nuget packages
   - FluentAssertions
   - Microsoft.AspNetCore.TestHost
 - write the test


```csharp
using FluentAssertions;
using FooApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FooApiFunctionalTests
{
    [TestFixture]
    public class WeatherForecastControllerTest
    {
        [Test]
        public async Task WeatherForecastControllerTestAsync()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseStartup<Startup>();
                    webHost.UseConfiguration(configuration);
                    webHost.UseTestServer();
                    webHost.ConfigureTestServices(services =>
                    {
                        services.AddLogging(config => config.AddConsole()); 
                    });
                });
            
            var host = await hostBuilder.StartAsync();

            var client = host.GetTestClient();

            var response = await client.GetAsync("WeatherForecast");
            var items = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
            items.Should().NotBeEmpty();
        }
        
    }
}
```


```cmd
PS C:\Github\aspnetcore-recipes\testing\01-functionaltests\src> dotnet test
  Determining projects to restore...
  Restored C:\Github\aspnetcore-recipes\testing\01-functionaltests\src\FooApi.csproj (in 309 ms).
  Restored C:\Github\aspnetcore-recipes\testing\01-functionaltests\tests\FooApiTests.csproj (in 795 ms).
  FooApi -> C:\Github\aspnetcore-recipes\testing\01-functionaltests\src\bin\Debug\net5.0\FooApi.dll
  FooApiTests -> C:\Github\aspnetcore-recipes\testing\01-functionaltests\tests\bin\Debug\net5.0\FooApiTests.dll
Test run for C:\Github\aspnetcore-recipes\testing\01-functionaltests\tests\bin\Debug\net5.0\FooApiTests.dll (.NETCoreApp,Version=v5.0)
Microsoft (R) Test Execution Command Line Tool Version 16.8.3
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 628 ms - FooApiTests.dll (net5.0)

```

