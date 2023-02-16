using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Service
{
    public class SettlementService : ISettlementService
    {
        public IEnumerable<SettlementAggregation> CalculateSettlementAggregation(string settlementLocationName)
        {
            var result = ExcelReader.ReadFromExcel()
               .Where(x => settlementLocationName == null || x.SettlementLocationName == settlementLocationName)
               .GroupBy(x => new { x.SettlementLocationName, x.DateOfService })
               .Select(grp =>
               {
                   var totalEntries = grp.Count();
                   return new SettlementAggregation()
                   {
                       settlement_location = grp.Key.SettlementLocationName,
                       year_date = grp.Key.DateOfService,
                       avg_price = grp.Sum(x => x.PricePerMWh) / totalEntries,
                       avg_volume = grp.Sum(x => x.VolumeMWh) / totalEntries,
                       min_price = grp.Min(x => x.PricePerMWh),
                       max_price = grp.Min(x => x.PricePerMWh),
                   };
               });

            return result;
        }

        public IEnumerable<Settlement> GetSettlementLocations(DateTime start_date, DateTime end_date, string settlementLocationName = null)
        {
           return ExcelReader.ReadFromExcel()
                .Where(x => x.DateOfService >= start_date &&
                       x.DateOfService <= end_date &&
                       (settlementLocationName == null || x.SettlementLocationName == settlementLocationName))
                .OrderBy(x => x.DateOfService)
                .Select(x => new Settlement()
                {
                    SettlementLocationName = x.SettlementLocationName,
                    DateOfService= x.DateOfService,
                    PricePerMWh = x.PricePerMWh,
                    VolumeMWh= x.VolumeMWh,
                }).ToList();
        }
    }
}
