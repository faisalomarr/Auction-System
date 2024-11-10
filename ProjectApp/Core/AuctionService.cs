using ProjectApp.Core.Interfaces;

namespace ProjectApp.Core;

public class AuctionService : IAuctionService
{
    
    private readonly IAuctionPersistence _auctionPersistence;
    public AuctionService(IAuctionPersistence auctionPersistence)
    {
        _auctionPersistence = auctionPersistence;
    }
    
    public List<Auction> GetAuctions()
    {
        List<Auction> auctions = new List<Auction>();
        List<Auction> auctionsStillinBid = new List<Auction>();
        auctions=_auctionPersistence.GetAuctions();
        foreach (Auction auction in auctions)
        {
            if (auction.AuctionEndTime > DateTime.Now)
            {
                auctionsStillinBid.Add(auction);
            }
        }
        auctionsStillinBid.Sort((a1, a2) => a1.AuctionEndTime.CompareTo(a2.AuctionEndTime));
        return auctionsStillinBid;
    }

    public List<Auction> GetAuctionsWhereBid(string username)
    {
        List<Auction> auctions = new List<Auction>();
        List<Auction> auctionsStillInBid = new List<Auction>();
        auctions = _auctionPersistence.GetAuctionsWhereBid(username);
        foreach (Auction auction in auctions)
        {
            if (auction.AuctionEndTime > DateTime.Now)
            {
                auctionsStillInBid.Add(auction);
            }
        }
        return auctionsStillInBid;
        
    }

    

    public void AddAuction(String username, String title , decimal price, DateTime endDate, String description)
    {
        if (username == null || title == null || title == "")
        {
            throw new ArgumentNullException();
        }
        Auction auction = new Auction(-10,title,description,price,endDate,username);
        _auctionPersistence.AddAuction(auction);
    }

    public void ChangeAuctionDecription(int auctionId, string description, String username)
    {
        if (auctionId < 1 || description == null || username == null)
        {
            throw new ArgumentNullException();
        }
        
        _auctionPersistence.ChangeAuctionDescription(auctionId, description,username);
    }

    public Auction GetAuctionById(int auctionId)
    {
        
        Auction auction = _auctionPersistence.GetAuctionById(auctionId);
        if (auction == null)
        {
            throw new ArgumentException("Auction not found");
        }

        return auction;
    }

    public List<Auction> GetAuctionsOfUser(string username)
    {
        List<Auction> auctions = _auctionPersistence.GetAuctionsOfUser(username);
        return auctions;

    }

    public List<Auction> GetAuctionsToBid(string username)
    {
        List<Auction> auctions = new List<Auction>();
        List<Auction> auctionsStillinBid = new List<Auction>();

        auctions = _auctionPersistence.GetAuctionsToBid(username);
        foreach (Auction auction in auctions)
        {
            if (auction.AuctionEndTime > DateTime.Now)
            {
                auctionsStillinBid.Add(auction);
            }
        }
        auctionsStillinBid.Sort((a1, a2) => a1.AuctionEndTime.CompareTo(a2.AuctionEndTime));
        return auctionsStillinBid;
    }

    public List<Auction> GetAuctionsWon(string username)
    {
        if (username == null)
        {
            throw new ArgumentNullException();
        }
       return _auctionPersistence.GetAuctionsWon(username);
    }

    public void AddBid(string username, decimal bid, int auctionId)
    {
        if (auctionId < 1 || bid < 0 || username == null)
        {
            throw new ArgumentNullException();
        }
        Console.WriteLine("test");

        _auctionPersistence.AddBid(auctionId, bid, username);
    }
    
    
}