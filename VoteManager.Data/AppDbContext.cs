using Microsoft.EntityFrameworkCore;

namespace VoteManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }


    }
}