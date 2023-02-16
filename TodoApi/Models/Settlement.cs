

using System;

namespace TodoApi.Models
{
    public class Settlement
    {
        public string SettlementLocationName { get; set; }

        public int SettlementLocationID { get; set; }

        public DateTime DateOfService { get; set; }

        public double PricePerMWh { get; set; }

        public double VolumeMWh { get; set; }
        
        public DateTime InsertDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
