using AuctionService.Entities.DTOs;

namespace AuctionService.Interfaces;

public interface IAuctionRepository
{
    Task<AuctionDto> GetAuctionByIdAsync(Guid id);
    Task<List<AuctionDto>> GetAuctionsAsync();
    Task<List<AuctionDto>> GetAuctionsFromDate(DateTime dateTime);
}
