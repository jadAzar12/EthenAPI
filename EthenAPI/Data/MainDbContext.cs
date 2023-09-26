using EthenAPI.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace EthenAPI.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
       public DbSet<AssetMetaData> AssetMetaData { get; set; }
    }
}
