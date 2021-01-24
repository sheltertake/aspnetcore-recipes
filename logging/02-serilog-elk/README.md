# Logging

## 02 - Serilog + Elk

Copy and paste 01-serilog renaming 02-elk

### Configure local ELK

 - git clone https://github.com/deviantony/docker-elk.git
 - edit  elasticsearch/config/elasticsearch.yml
 - replace xpack.license.self_generated.type: trial with basic


```yml
---
## Default Elasticsearch configuration from Elasticsearch base image.
## https://github.com/elastic/elasticsearch/blob/master/distribution/docker/src/docker/config/elasticsearch.yml
#
cluster.name: "docker-cluster"
network.host: 0.0.0.0

## X-Pack settings
## see https://www.elastic.co/guide/en/elasticsearch/reference/current/setup-xpack.html
#
xpack.license.self_generated.type: basic
xpack.security.enabled: true
xpack.monitoring.collection.enabled: true

```

 - docker-compose up
 - open http://localhost:5601/ and login

```text
The stack is pre-configured with the following privileged bootstrap user:

user: elastic
password: changeme
```
 - configure index pattern

### Configure logstash.conf

### Install http serilog sinks + enrichers

```xml
    <PackageReference Include="Serilog" Version="2.10.0" />
    <PackageReference Include="Serilog.Settings.Configuration" Version="3.1.0" />
    
    <PackageReference Include="Serilog.AspNetCore" Version="3.4.0" />
    
    <PackageReference Include="Serilog.Sinks.Http" Version="7.2.0" />
    <PackageReference Include="Serilog.Sinks.Console" Version="3.1.1" />
        
    <PackageReference Include="Serilog.Enrichers.Environment" Version="2.1.3" />
    <PackageReference Include="Serilog.Enrichers.Process" Version="2.0.1" />
    <PackageReference Include="Serilog.Enrichers.Thread" Version="3.1.0" />
    <PackageReference Include="Serilog.Enrichers.Memory" Version="1.0.4" />
    <PackageReference Include="Serilog.Enrichers.AssemblyName" Version="1.0.9" />
```

### Test

 - navigate http://localhost:5001/weatherforecast
### Links

 - [docker elk](https://github.com/deviantony/docker-elk)
 - [serilog logstash](https://www.devground.co/blog/posts/serilog-sending-logs-to-logstash/)

### Configuration via code 

```csharp

 Log.Logger = new LoggerConfiguration()
    //.ReadFrom.Configuration(Configuration)
    .MinimumLevel.Is(LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
            requestUri: "http://localhost:5000",
            batchFormatter: new Serilog.Sinks.Http.BatchFormatters.ArrayBatchFormatter(),
            bufferBaseFileName: $"C:\\Temp\\elk-serilog\\Buffer-{AppDomain.CurrentDomain.FriendlyName}"
        )
    .WriteTo.Console()
    .EnrichMe()
    .CreateLogger();

```
