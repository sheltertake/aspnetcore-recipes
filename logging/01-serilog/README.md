# Logging

## 01 - Serilog

```cmd
dotnet new webapi -n FooApi -o src/
```

### Serilog enrichers and sinks

```xml
<PackageReference Include="Serilog.Sinks.Console" Version="3.1.1" />
<PackageReference Include="Serilog" Version="2.10.0" />
<PackageReference Include="Serilog.AspNetCore" Version="3.4.0" />
<PackageReference Include="Serilog.Settings.Configuration" Version="3.1.0" />
<PackageReference Include="Serilog.Enrichers.Environment" Version="2.1.3" />
<PackageReference Include="Serilog.Enrichers.Process" Version="2.0.1" />
<PackageReference Include="Serilog.Enrichers.Thread" Version="3.1.0" />
<PackageReference Include="Serilog.Enrichers.Memory" Version="1.0.4" />
<PackageReference Include="Serilog.Enrichers.AssemblyName" Version="1.0.9" />
```

### Configuration

 - program.cs

```csharp

using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FooApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .EnrichMe()
                .CreateLogger();
            try
            {
                Log.Information("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static LoggerConfiguration EnrichMe(this LoggerConfiguration logger) => logger
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyVersion()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.WithMemoryUsage();
    }
}


```

 - startup.cs configure

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FooApi v1"));
    }
    
    app.UseSerilogRequestLogging(); // <-- Add this line

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

 - appsettings.json

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "Microsoft.AspNetCore": "Warning",
        "System": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
        //"Args": {
        //  "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
        //}
      }
    ]
  },
  "AllowedHosts": "*"
}
```

### Links

 - [Logging in ASP.NET Core 5 using Serilog](https://www.ezzylearning.net/tutorial/logging-in-asp-net-core-5-using-serilog)
 - [Serilog configuration sample](https://github.com/serilog/serilog-docker/blob/master/web-sample/src/Program.cs)
 - [Serilog getting started](https://github.com/serilog/serilog/wiki/Getting-Started)
 - [Serilog aspnetcore](https://github.com/serilog/serilog-aspnetcore)