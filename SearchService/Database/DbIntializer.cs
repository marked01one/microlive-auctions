using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Database;

public class DbIntializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
        
        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        int count = (int) await DB.CountAsync<Item>();

        using IServiceScope scope = app.Services.CreateScope();

        AuctionServiceHttpClient httpClient = scope.ServiceProvider
            .GetRequiredService<AuctionServiceHttpClient>();

        List<Item> items = await httpClient.GetItemsForSearchDbAsync();

        Console.WriteLine(items.Count + " returned from AuctionService!");

        if (items.Count > 0) await DB.SaveAsync(items);
    }
}