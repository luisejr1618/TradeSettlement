using System;

namespace TodoApi.Models
{
    public class SettlementAggregation
    {
        public string settlement_location { get; set; }
        public DateTime year_date { get; set; }

        public double avg_price { get; set; }

        public double avg_volume { get; set; }

        public double avg_total_dollars => avg_price * avg_volume;

        public double min_price { get; set; }

        public double max_price { get; set; }
    }
}
