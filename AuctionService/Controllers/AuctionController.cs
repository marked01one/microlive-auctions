using AuctionService.Database;
using AuctionService.Entities.DTOs;
using AuctionService.Entities.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    ///     A GET endpoint to get all auctions ever made
    /// </summary>
    /// <returns>
    ///     A list of auction objects
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
    {
        IQueryable<Auction> query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date)) {
            query = query.Where(x => 
                x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    /// <summary>
    ///     A GET endpoint to get a specific auction that matches the given ID
    /// </summary>
    /// <param name="id">The Guid ID of the auction</param>
    /// <returns>The auction object that contains the given ID</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        Auction auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();
        
        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        Auction auction = _mapper.Map<Auction>(createAuctionDto);
        // TODO: Add current user as seller
        auction.Seller = "test";

        _context.Auctions.Add(auction);

        bool result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to database!");
        
        return CreatedAtAction(
            nameof(GetAuctionById), new {auction.Id}, 
            _mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        Auction auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (auction == null) return NotFound();

        // TODO: Check seller == username

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        bool result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Problem saving changes!");

        return Ok();
    }

    /// <summary>
    ///     Delete the indicated auction
    /// </summary>
    /// <param name="id">The Guid ID of the auction</param>
    /// <returns>A `ActionResult` HTTP response</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        Auction auction = await _context.Auctions.FindAsync(id);

        if (auction == null) return NotFound();

        // TODO: check seller == username

        _context.Auctions.Remove(auction);

        bool result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update database!");

        return Ok();
    }

}
