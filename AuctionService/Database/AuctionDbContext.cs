using AuctionService.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Database;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Auction> Auctions { get; set; }

}
