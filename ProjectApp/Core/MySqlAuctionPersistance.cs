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

    public void AddAuction()
    {
        throw new NotImplementedException();
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
}