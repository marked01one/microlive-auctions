namespace SearchService.Entities.Events;

public class AuctionUpdated
{
    public string Make { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public int Year { get; set; }
}