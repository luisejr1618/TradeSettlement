using System;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Models;
using TodoApi.Service;
using Xunit;

namespace TodoApi.Tests
{
    public class TradesTest
    {
        private TestServer _server;

        public HttpClient Client { get; private set; }

        public TradesTest()
        {
            SetUpClient();
        }

        [Theory]
        [InlineData("buy",1,"AC",28,162, 1591514264000,StatusCodes.Status201Created)]
        [InlineData("sell",1,"AC",28,162, 1591514264000,StatusCodes.Status201Created)]
        [InlineData("buy",1,"AC",101,162, 1591514264000,StatusCodes.Status400BadRequest)]
        [InlineData("buy",1,"AC",0,162, 1591514264000,StatusCodes.Status400BadRequest)]
        [InlineData("store",1,"AC",0,162, 1591514264000,StatusCodes.Status400BadRequest)]
        public async Task Trades_CreateTrade_Test(
            string type,
            int userId,
            string symbol,
            int shares,
            int price,
            long timestamp,
            int expectedResult)
        {
            var content = JsonSerializer.Serialize(new Trade()
            {
                Type= type,
                UserID= userId,
                Symbol= symbol,
                Shares=shares,
                Price=price, 
                Timestamp=timestamp,
            });

            var response = await Client.PostAsync(
                "/api/Trades",
                new StringContent(content, Encoding.UTF8, "application/json")
            );

            response.StatusCode.Should().BeEquivalentTo(expectedResult);
        }


        [Theory]
        [InlineData("1", true, StatusCodes.Status200OK)]
        [InlineData("3", false, StatusCodes.Status404NotFound)]
        public async Task Trades_GetTrade_Test(
            string id,
            bool createTrade,
            int expectedResult)
        {
            var content = JsonSerializer.Serialize(new Trade()
            {
                Type = "buy",
                UserID = 1,
                Symbol = "AC",
                Shares = 28,
                Price = 162,
                Timestamp = 1591514264000,
            });

            if (createTrade)
            {
                _ = await Client.PostAsync(
                    "/api/Trades",
                    new StringContent(content, Encoding.UTF8, "application/json")
                );
            }

            var response = await Client.GetAsync(
                $"/api/Trades/{id}"
            );

            response.StatusCode.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("","", StatusCodes.Status200OK)]
        [InlineData("1","", StatusCodes.Status200OK)]
        [InlineData("","sell", StatusCodes.Status200OK)]
        [InlineData("123","sell", StatusCodes.Status200OK)]
        public async Task Trades_GetTrades_Test(
            string userId,
            string type,
            int expectedResult)
        {
            var response = await Client.GetAsync(
                $"/api/Trades?userID={userId}&type={type}"
            );

            response.StatusCode.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task Trades_DeleteTrade_Test()
        {
            var response = await Client.DeleteAsync(
                $"/api/Trades/1"
            );

            response.StatusCode.Should().BeEquivalentTo(StatusCodes.Status405MethodNotAllowed);
        }

        [Fact]
        public async Task Trades_PutTrade_Test()
        {
            var response = await Client.PutAsync(
                $"/api/Trades/1",
                new StringContent("", Encoding.UTF8, "application/json")
            );

            response.StatusCode.Should().BeEquivalentTo(StatusCodes.Status405MethodNotAllowed);
        }

        [Fact]
        public async Task Trades_PatchTrade_Test()
        {
            var response = await Client.PatchAsync(
                $"/api/Trades/1",
                new StringContent("", Encoding.UTF8, "application/json")
            );

            response.StatusCode.Should().BeEquivalentTo(StatusCodes.Status405MethodNotAllowed);
        }

        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ITradesService, TradesService>();
                    services.AddDbContext<TradesContext>(opt =>
                       opt.UseInMemoryDatabase("Trades"), ServiceLifetime.Singleton);
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }
    }
}
