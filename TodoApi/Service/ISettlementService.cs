using System;
using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Service
{
    public interface ISettlementService
    {
        IEnumerable<Settlement> GetSettlementLocations(DateTime start_date, DateTime end_date, string settlementLocationName);
        IEnumerable<SettlementAggregation> CalculateSettlementAggregation( string settlementLocationName);
    }
}
