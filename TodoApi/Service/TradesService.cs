using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Service
{
    public class TradesService : ITradesService
    {
        private readonly TradesContext _context;

        public TradesService(TradesContext context)
        {
            _context = context;
        }

        public async Task<Trade> Create(Trade trade)
        {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();

            return trade;
        }

        public async Task<Trade> GetTrade(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task<IEnumerable<Trade>> GetTrades(int? userID, string type)
        {
            return await _context.Trades.Where(x => (!userID.HasValue || x.UserID == userID) &&
                                              (type == null || x.Type == type)).OrderBy(x => x.Id).ToListAsync();
        }
    }
}