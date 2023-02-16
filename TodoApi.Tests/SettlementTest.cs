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
    public class SettlementTest
    {
        private TestServer _server;

        public HttpClient Client { get; private set; }

        public SettlementTest()
        {
            SetUpClient();
        }

        [Theory]
        [InlineData("2022-01-31", "2022-01-31", "", StatusCodes.Status200OK)]
        [InlineData("2022-01-31", "2022-01-31", "Test123", StatusCodes.Status200OK)]
        [InlineData("2022-01-31", "01-01-0001", "Test123", StatusCodes.Status400BadRequest)]
        [InlineData("01-01-0001", "2022-01-31", "Test123", StatusCodes.Status400BadRequest)]
        [InlineData("01-01-0001", "01-01-0001", "", StatusCodes.Status400BadRequest)]
        public async Task Settlement_RetSettlementDates_Test(
            string startDate,
            string endDate,
            string settlementName,
            int expectedResult)
        {
            var response = await Client.GetAsync(
                $"/api/Settlement/ret-settlement-dates/{startDate}/{endDate}?settlementName={settlementName}"
            );

            response.StatusCode.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("South", "2022-06-01", 56.34, 180.2916139, 10157.629527126, 56.34, 56.34, StatusCodes.Status200OK, true)]
        [InlineData("Houston", "2023-06-01", 5.99, 1330.226171, 7968.05476429, 5.99, 5.99, StatusCodes.Status200OK, true)]
        [InlineData("North", "2024-09-01", 14.89, 2132.443445, 31752.08289605, 14.89, 14.89, StatusCodes.Status200OK, false)]
        [InlineData("North", "2022-10-01", 74.82, 129.217786, 9668.074748519999, 74.82, 74.82, StatusCodes.Status200OK, true)]
        public async Task Settlement_AggMonthly_Test(
            string settlementLocation,
            string yearDate,
            double avgPrice,
            double avgVolume,
            double avgTotalDollar,
            double minPrice,
            double maxPrice,
            int expectedResult,
            bool useSettlementLocationArg)
        {

            var settlementLocationName = useSettlementLocationArg ? settlementLocation : "";

            var response = await Client.GetAsync(
                $"/api/Settlement/agg-monthly?settlementName={settlementLocationName}"
            );

            response.StatusCode.Should().BeEquivalentTo(expectedResult);

            var resultResponse = await response.Content.ReadAsStringAsync();
            var resultObject = JsonSerializer.Deserialize<SettlementAggregation[]>(resultResponse);
            var result = resultObject.FirstOrDefault(x => x.year_date == DateTime.Parse(yearDate) && x.settlement_location == settlementLocation);

            result.Should().NotBeNull();
            result.avg_price.Should().Be(avgPrice);
            result.avg_volume.Should().Be(avgVolume);
            result.avg_total_dollars.Should().Be(avgTotalDollar);
            result.min_price.Should().Be(minPrice);
            result.max_price.Should().Be(maxPrice);
        }


        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ISettlementService, SettlementService>();
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }
    }
}
