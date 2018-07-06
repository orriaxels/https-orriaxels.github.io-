using Microsoft.EntityFrameworkCore;
using API.Models.EntityModels;

namespace API.Repositories
{
     public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {}

        public DbSet<Player> Player { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<GameInfo> GamesWon { get; set; }
    }
}
