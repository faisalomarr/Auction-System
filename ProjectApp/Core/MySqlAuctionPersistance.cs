using System.Data;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Core.Interfaces;
using ProjectApp.Persistance;

namespace ProjectApp.Core;

public class MySqlAuctionPersistance : IAuctionPersistance
{
    private readonly AuctionDbContext auctionDbContext;

    public MySqlAuctionPersistance(AuctionDbContext auctionDbContext)
    {
        this.auctionDbContext = auctionDbContext;
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
            select auction).ToList();

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

    public List<Auction> GetAuctionsWhereBidHighest(string username)
    {
        return null;
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

        // Handle the case where the auction does not exist
        if (auctionDb == null) throw new DataException("Auction not found");

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
                BidId = bidDb.BidId,
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

}