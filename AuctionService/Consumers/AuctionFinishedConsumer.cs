using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Contracts;
using AuctionService.Database;
using AuctionService.Entities.Models;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext _dbContext;

    public AuctionFinishedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("---> Consuming `AuctionFinished`...");

        Auction auction =  await _dbContext.Auctions.FindAsync(context.Message.AuctionId);
        if (context.Message.ItemSold) {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }

        // Check whether the final bid for the auction is above the reserve price or not
        auction.Status = auction.SoldAmount > auction.ReservePrice 
            ? Status.Finished : Status.ReserveNotMet;

        await _dbContext.SaveChangesAsync();
    }
}
