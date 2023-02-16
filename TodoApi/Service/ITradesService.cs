using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Service
{
    public interface ITradesService
    {
        Task<Trade> Create(Trade trade);

        Task<IEnumerable<Trade>> GetTrades(int? userID, string type);

        Task<Trade> GetTrade(int id);
    }
}
