using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Entities.Models;
using SearchService.Helpers;
using ZstdSharp.Unsafe;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IList<Item>>> SearchItems([FromQuery]RequestParams requestParams)
    {
        PagedSearch<Item, Item> query = DB.PagedSearch<Item, Item>();

        query.Sort(x => x.Ascending(auction => auction.Make));

        if (!string.IsNullOrEmpty(requestParams.SearchTerm)) {
            query.Match(Search.Full, requestParams.SearchTerm).SortByTextScore();
        }

        query = requestParams.OrderBy switch 
        {
            "make" => query.Sort(x => x.Ascending(auction => auction.Make)),
            "new" => query.Sort(x => x.Descending(auction => auction.CreatedAt)),
            _ => query.Sort(x => x.Ascending(auction => auction.AuctionEnd)) 
        };

        query = requestParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => 
                x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow
            ),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(requestParams.Winner)) {
            query.Match(x => x.Winner == requestParams.Winner);
        }

        if (!string.IsNullOrEmpty(requestParams.Seller)) {
            query.Match(x => x.Seller == requestParams.Seller);
        }
        
        (IReadOnlyList<Item> results, long pageCount, int totalCount) = await query.ExecuteAsync(); 

        return Ok(new 
        {
            results = results,
            pageCount = pageCount,
            totalCount = totalCount 
        });
    }
}
