namespace TodoApi.Models
{
    public class Trade
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int UserID { get; set; }

        public string Symbol { get; set; }

        public int Shares { get; set; }

        public int Price { get; set; }

        public long Timestamp { get; set; }
    }

    public enum TradeType 
    {
        buy,
        sell,
    }
}
