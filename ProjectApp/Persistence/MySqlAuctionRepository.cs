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
        // Step 1: Retrieve all AuctionDb objects from the database
        List<AuctionDb> auctionDbs = auctionDbContext.AuctionsDbs.ToList();

        // Step 2: Convert each AuctionDb to an Auction using a constructor
        List<Auction> auctions = auctionDbs.Select(auctionDb => new Auction(
            auctionDb.Id,
            auctionDb.Name,
            auctionDb.Description,
            auctionDb.StartPrice,
            auctionDb.AuctionEndTime,
            auctionDb.Username
        )).ToList();

        // Step 3: Return the list of Auction objects
        return auctions;
    }

    public List<Auction> GetAuctionsWhereBid(string username)
    {
        List<AuctionDb> auctionDbs = (from bid in auctionDbContext.BidDbs
                join auction in auctionDbContext.AuctionsDbs 
                    on bid.AuctionId equals auction.Id
                where bid.username == username
                select auction)
            .GroupBy(a => a.Id)
            .Select(g => g.First())
            .ToList();

        // Step 2: Convert each AuctionDb to an Auction using a constructor
        List<Auction> auctions = auctionDbs.Select(auctionDb => new Auction(
            auctionDb.Id,
            auctionDb.Name,
            auctionDb.Description,
            auctionDb.StartPrice,
            auctionDb.AuctionEndTime,
            auctionDb.Username
        )).ToList();

        // Step 3: Return the list of Auction objects
        return auctions;
        
    }


    // Implementing AddAuction
    public void AddAuction(Auction auction)
    {
        try
        {
            // Convert Auction object to AuctionDb
            var auctionDb = new AuctionDb
            {
                Name = auction.Name,
                Description = auction.Description,
                StartPrice = auction.StartPrice,
                AuctionEndTime = auction.AuctionEndTime,
                Username = auction.Username
            };

            // Add auction to the DbContext and save changes
            auctionDbContext.AuctionsDbs.Add(auctionDb);
            auctionDbContext.SaveChanges(); // Persist changes to the database
            
        }
        catch (Exception ex)
        {
            // Handle the error (e.g., log it) and return false to indicate failure
            Console.WriteLine(ex.Message);
            
        }
    }

    public void ChangeAuctionDescription(int auctionId, string newDescription, string username)
    {
        try
        {
            // Find the auction by ID
            var auctionDb = auctionDbContext.AuctionsDbs.FirstOrDefault(a => a.Id == auctionId);
        
            // Check if the auction exists
            if (auctionDb == null)
            {
                Console.WriteLine($"Auction with ID {auctionId} not found.");
                return;
            }

            if (!auctionDb.Username.Equals(username))
            {
                Console.WriteLine($"username does not own auction with ID {auctionId}");
                return;
            }

            // Update the auction's description
            auctionDb.Description = newDescription;

            // Save the changes to the database
            auctionDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            // Handle the error (e.g., log it) and indicate failure
            Console.WriteLine($"Error updating auction description: {ex.Message}");
        }
    }


    public Auction GetAuctionById(int id)
    {
        // Retrieve the auction from the database, including related bids
        AuctionDb? auctionDb = auctionDbContext.AuctionsDbs
            .Where(a => a.Id == id)
            .Include(a => a.BidDbs) // Eager loading of related bids
            .FirstOrDefault();

        // Handle the case where the auction does not exist or time passed
        if (auctionDb == null || auctionDb.AuctionEndTime<=DateTime.Now) throw new DataException("Auction not found");

        // Manually map fields from AuctionDb to Auction
        Auction auction = new Auction
        {
            Id = auctionDb.Id,
            Name = auctionDb.Name,
            Description = auctionDb.Description,
            StartPrice = auctionDb.StartPrice,
            AuctionEndTime = auctionDb.AuctionEndTime,
            Username = auctionDb.Username,
        };

        // Sort bids by amount in descending order and map them to the Auction object
        var sortedBids = auctionDb.BidDbs
            .OrderByDescending(bidDb => bidDb.Amount) // Sorting by Amount
            .Select(bidDb => new Bid
            {
                Id = bidDb.Id,
                Amount = bidDb.Amount,
                TimeOfBid = bidDb.TimeOfBid,
                username = bidDb.username,
            });

        // Add each sorted bid to the Auction object
        foreach (var bid in sortedBids)
        {
            auction.AddBid(bid); // Using the AddBid method to add each bid
        }

        return auction;
    }

    public List<Auction> GetAuctionsOfUser(string username)
    {
        // Step 1: Retrieve AuctionDb records where the Username matches, without including Bid data
        List<AuctionDb> auctionDbs = auctionDbContext.AuctionsDbs
            .Where(auction => auction.Username == username) // Filter by username
            .ToList();

        // Step 2: Convert each AuctionDb to an Auction without including bids
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
    }

    public List<Auction> GetAuctionsToBid(string username)
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
    }
    
    
    public List<Auction> GetAuctionsWon(string username)
    {
        // Retrieve auctions where the auction has ended and the user placed the highest bid
        var wonAuctions = auctionDbContext.AuctionsDbs
            .Where(a => a.AuctionEndTime < DateTime.Now && a.BidDbs.Any()) // Only past auctions with bids
            .Select(a => new
            {
                Auction = a,
                HighestBid = a.BidDbs.OrderByDescending(b => b.Amount).FirstOrDefault() // Get the highest bid per auction
            })
            .Where(result => result.HighestBid != null && result.HighestBid.username == username) // Ensure user has the highest bid
            .Select(result => result.Auction) // Select the auction itself
            .ToList();

        // Map AuctionDb to Auction model for return
        return wonAuctions.Select(a => new Auction
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            StartPrice = a.StartPrice,
            AuctionEndTime = a.AuctionEndTime,
            Username = a.Username
        }).ToList();
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