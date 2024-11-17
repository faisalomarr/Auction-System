using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Core;
using ProjectApp.Core.Interfaces;

namespace ProjectApp.Persistence;

public class MySqlAuctionPersistence : IAuctionPersistence
{
    private readonly AuctionDbContext auctionDbContext;
    private readonly IMapper mapper;


    public MySqlAuctionPersistence(AuctionDbContext auctionDbContext, IMapper mapper)
    {
        this.auctionDbContext = auctionDbContext;
        this.mapper = mapper;
        
    }
    
    public List<Auction> GetAuctions()
    {
        var auctionDbs = auctionDbContext.AuctionsDbs
            .Where(a => a.AuctionEndTime> DateTime.Now)
            .OrderBy(a => a.AuctionEndTime)
            .ToList();
        
        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }
        
        return result;
    }

    public List<Auction> GetAuctionsWhereBid(string username)
    {
        var auctionDbs = auctionDbContext.AuctionsDbs
            .Where(auction => auctionDbContext.BidDbs
                .Any(bid => bid.username == username && bid.AuctionId == auction.Id) && auction.AuctionEndTime > DateTime.Now)
            .Distinct() 
            .ToList();


        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }
        
        return result;
        
    }

    
    public void AddAuction(Auction auction)
    {
        AuctionDb auctionDb = mapper.Map<AuctionDb>(auction);
        
        auctionDbContext.AuctionsDbs.Add(auctionDb);
        auctionDbContext.SaveChanges();
    }


    public void ChangeAuctionDescription(int auctionId, string newDescription, string username)
    {
        var auctionDb = auctionDbContext.AuctionsDbs
            .FirstOrDefault(a => a.Id == auctionId && a.Username == username);

        if (auctionDb == null){
            throw new ArgumentException("Auction not found or username does not own the auction");
        }

        auctionDb.Description = newDescription;

        auctionDbContext.SaveChanges();
        
    }


    public Auction GetAuctionById(int id)
    {
        AuctionDb? auctionDb = auctionDbContext.AuctionsDbs
            .Where(a => a.Id == id && a.AuctionEndTime > DateTime.Now)
            .Include(a => a.BidDbs.OrderByDescending(bidDb => bidDb.Amount)) 
            .FirstOrDefault();

        if (auctionDb == null) 
            throw new DataException("Auction not found");

        Auction auction = mapper.Map<Auction>(auctionDb);
        foreach (BidDb bidDb in auctionDb.BidDbs)
        {
            Bid bid = mapper.Map<Bid>(bidDb);
            auction.AddBid(bid);
        }

        return auction;
    }


    public List<Auction> GetAuctionsOfUser(string username)
    {
        List<AuctionDb> auctionDbs = auctionDbContext.AuctionsDbs
            .Where(auction => auction.Username == username) 
            .ToList();
        
        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }

        return result;
    }

    /*public List<Auction> GetAuctionsToBid(string username)
    {
        // Retrieve auctions that the specified user does not own
        List<AuctionDb> auctionDbs = auctionDbContext.AuctionsDbs
            .Where(auction => auction.Username != username) // Exclude user's own auctions
            .ToList();

        // Convert each AuctionDb to an Auction without including bids
        List<Auction> auctions = auctionDbs.Select(auctionDb => new Auction
        {
            Id = auctionDb.Id,
            Name = auctionDb.Name,
            Description = auctionDb.Description,
            StartPrice = auctionDb.StartPrice,
            AuctionEndTime = auctionDb.AuctionEndTime,
            Username = auctionDb.Username
        }).ToList();

        return auctions;
    }*/
    
    
    public List<Auction> GetAuctionsWon(string username)
    {
        var wonAuctions = auctionDbContext.AuctionsDbs
            .Where(a => a.AuctionEndTime < DateTime.Now &&
                        a.BidDbs.Any() &&
                        a.BidDbs.OrderByDescending(b => b.Amount).FirstOrDefault()!.username == username)
            .ToList();
        

        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in wonAuctions)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }

        return result;
    }
    

    
    
    public void AddBid( int auctionId, decimal bid ,string username)
    {
        // Find the auction in the database
        // Find the auction in the database
        var auctionDb = auctionDbContext.AuctionsDbs.FirstOrDefault(a => a.Id == auctionId);

// Check if the auction exists
        if (auctionDb == null)
        {
            Console.WriteLine($"Auction with ID {auctionId} not found.");
            return;
        }

// Retrieve all bids associated with the auction
        var auctionBids = auctionDbContext.BidDbs
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .ToList();

// Determine the current highest bid or use the starting price if there are no bids
        var highestBidAmount = auctionBids.FirstOrDefault()?.Amount ?? auctionDb.StartPrice;
        

        // Check if the user owns the auction
        if (auctionDb.Username.Equals(username))
        {
            Console.WriteLine($"User '{username}' cannot bid on their own auction with ID {auctionId}");
            return;
        }
        
        if (auctionDb.AuctionEndTime < DateTime.Now)
        {
            return;
        }

        // Determine the current highest bid or use the starting price if there are no bids
        
        // Check if the bid is higher than the current highest bid or starting price
        if (bid <= highestBidAmount)
        {
            throw new Exception("Gick ej");
        }

        // Create a new BidDb object
        var biddb = new BidDb
        { 
            username = username,
            Amount = bid,
            TimeOfBid = DateTime.Now,
            AuctionId = auctionDb.Id
        };

        // Update the auction's current price to the new highest bid
        auctionDb.StartPrice = bid; // Or use a 'CurrentPrice' property if you have one

        // Add the new bid to the auction's bids and save the changes
        auctionDbContext.BidDbs.Add(biddb); // Add the bid to the Bids table
        auctionDbContext.SaveChanges(); // Persist changes to the database

        Console.WriteLine($"Bid of {bid} added successfully for auction with ID {auctionId} by user {username}");
    }

}