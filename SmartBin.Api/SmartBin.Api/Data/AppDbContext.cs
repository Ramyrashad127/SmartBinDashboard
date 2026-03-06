using Microsoft.EntityFrameworkCore;
using SmartBin.Api.Models;

namespace SmartBin.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Bin> Bins { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<BinSection> BinSections { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SurroundingWaste> SurroundingWastes { get; set; }
        public DbSet<CrowdDensity> CrowdDensities { get; set; }
    }
}
