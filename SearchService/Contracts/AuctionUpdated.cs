namespace Contracts;
public class AuctionUpdated
{
    public Guid Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public int? Mileage { get; set; }
    public int? Year { get; set; }
}