using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDbAsync()
    {
        string lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(auction => auction.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        if (!string.IsNullOrEmpty(lastUpdated)) 
        {
            Console.WriteLine("`lastUpdated` string is empty!");
            lastUpdated = DateTime.UnixEpoch.ToString();
        };

        return await _httpClient.GetFromJsonAsync<List<Item>>(
            _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);
    }
}
