using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);

        Item item = await DB.Find<Item>().OneAsync(context.Message.Id);

        await item.DeleteAsync();
    }
}
