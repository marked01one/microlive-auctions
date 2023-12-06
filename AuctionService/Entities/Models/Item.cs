using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities.Models;

[Table("Items")]
public class Item
{
    public Guid Id { get; set; }
    public Guid AuctionId { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }
    public Auction Auction { get; set; }
}