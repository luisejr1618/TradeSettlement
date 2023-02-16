using Microsoft.EntityFrameworkCore;
using TodoApi.Service;

namespace TodoApi.Models
{
    public class TradesContext : DbContext
    {
        public TradesContext(DbContextOptions<TradesContext> options)
            : base(options)
        {
        }

        public DbSet<Trade> Trades { get; set; }
    }
}