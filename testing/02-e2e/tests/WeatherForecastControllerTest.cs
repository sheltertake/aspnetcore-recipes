using FluentAssertions;
using FooApiE2eTests.Proxies.FooApi;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FooApiE2eTests
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
    }
}