# Testing

## Recipe 02 - E2e tests

```cmd
dotnet new webapi -n FooApi -o src/
dotnet new nunit -n FooApiE2eTests -o tests/
```

 - install nuget packages
   - FluentAssertions
   - Microsoft.Extensions.Configuration.Json

 - add appsettings.json

```json
{
  "AppSettings": {
    "BaseUrl": "https://localhost:5001/"
  }
}
```

 - configure csproj


```xml
  <ItemGroup>
    <None Remove="appsettings.json" />
  </ItemGroup>

  <ItemGroup>
    <Content Include="appsettings.json">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
```

 - install NSwagStudio
   - create new nswag setting file and save it
   - tick CSharpClient and customize
     - namespace -> FooApiE2eTests.Proxies.FooApi
     - class name -> FooApiClient
     - operation generation mode -> single client from operation id 
     - generate optional parameters
     - wrap success responses
     - output file -> .\Proxies\FooApiClient.cs

 - write tests
   - test WeatherForecast using classic http client
   - test WeatherForecast using typed client

```csharp
[Test]
public async Task WeatherForecastControllerTestAsync()
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

    var baseUrl = configuration["AppSettings:BaseUrl"];


    using (var client = new HttpClient()
    {
        BaseAddress = new Uri(baseUrl)
    })
    {
        var response = await client.GetAsync("WeatherForecast");
        response.IsSuccessStatusCode.Should().BeTrue();
        var items = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
        items.Should().NotBeEmpty();
    }
    
}

[Test]
public async Task WeatherForecastControllerTestUsingTypedClientAsync()
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

    var baseUrl = configuration["AppSettings:BaseUrl"];

    var client = new FooApiClient(baseUrl, new HttpClient());
    var response = await client.WeatherForecastAsync();
    response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    response.Result.Should().NotBeEmpty();
}
```

  - run api & run tests

```cmd
PS C:\Github\aspnetcore-recipes\testing\02-e2e\src> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Github\aspnetcore-recipes\testing\02-e2e\src

PS C:\Github\aspnetcore-recipes\testing\02-e2e\tests> dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  FooApiE2eTests -> C:\Github\aspnetcore-recipes\testing\02-e2e\tests\bin\Debug\net5.0\FooApiE2eTests.dll
Test run for C:\Github\aspnetcore-recipes\testing\02-e2e\tests\bin\Debug\net5.0\FooApiE2eTests.dll (.NETCoreApp,Version=v5.0)
Microsoft (R) Test Execution Command Line Tool Version 16.8.3
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 686 ms - FooApiE2eTests.dll (net5.0)
PS C:\Github\aspnetcore-recipes\testing\02-e2e\tests>
```

