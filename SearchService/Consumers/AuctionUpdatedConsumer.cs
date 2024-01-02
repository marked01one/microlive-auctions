using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        AuctionUpdated auction = context.Message;
        
        Console.WriteLine("--> Consuming auction updated: " + auction.Id);

        Item item = await DB.Find<Item>().OneAsync(auction.Id);

        item.Make = auction.Make ?? item.Make;
        item.Model = auction.Model ?? item.Model;
        item.Color = auction.Color ?? auction.Color;
        item.Mileage = auction.Mileage ?? item.Mileage;
        item.Year = auction.Year ?? item.Year;

        await item.SaveAsync();
    }
}
