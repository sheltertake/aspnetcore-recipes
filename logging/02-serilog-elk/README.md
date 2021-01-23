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

### Install http serilog sink

```xml
<PackageReference Include="Serilog.Sinks.Http" Version="7.2.0" />
```

### Test


### Links

 - [docker elk](https://github.com/deviantony/docker-elk#how-to-disable-paid-features)
 - [serilog logstash](https://www.devground.co/blog/posts/serilog-sending-logs-to-logstash/)