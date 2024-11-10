using Microsoft.EntityFrameworkCore;
using ProjectApp.Core;

namespace ProjectApp.Persistence;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }
    
    public DbSet<AuctionDb> AuctionsDbs { get; set; }
    
    public DbSet<BidDb> BidDbs { get; set; }
    
}