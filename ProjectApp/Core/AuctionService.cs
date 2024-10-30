using ProjectApp.Core.Interfaces;

namespace ProjectApp.Core;

public class AuctionService : IAuctionService
{

    private IAuctionPersistance _auctionPersistance;

    public AuctionService(IAuctionPersistance auctionPersistance)
    {
        _auctionPersistance = auctionPersistance;
    }
    public List<Auction> GetAuctions()
    {
        List<Auction> auctions = new List<Auction>();
        List<Auction> auctionsStillinBid = new List<Auction>();
        auctions=_auctionPersistance.GetAuctions();
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
        auctions = _auctionPersistance.GetAuctionsWhereBid(username);
        foreach (Auction auction in auctions)
        {
            if (auction.AuctionEndTime > DateTime.Now)
            {
                auctionsStillInBid.Add(auction);
            }
        }
        return auctionsStillInBid;
        
    }

    

    public void AddAuction(String username, String title , decimal price, DateTime endDate)
    {
        if (username == null || title == null || title == "")
        {
            throw new ArgumentNullException();
        }
        Auction auction = new Auction(-10,title,"",price,endDate,username);
        _auctionPersistance.AddAuction(auction);
    }

    public void ChangeAuctionDecription(int auctionId, string description, String username)
    {
        if (auctionId < 1 || description == null || username == null)
        {
            throw new ArgumentNullException();
        }
        
        _auctionPersistance.ChangeAuctionDescription(auctionId, description,username);
    }

    public Auction GetAuctionById(int auctionId)
    {
        
        Auction auction = _auctionPersistance.GetAuctionById(auctionId);
        if (auction == null)
        {
            throw new ArgumentException("Auction not found");
        }

        return auction;
    }

    public List<Auction> GetAuctionsOfUser(string username)
    {
        List<Auction> auctions = _auctionPersistance.GetAuctionsOfUser(username);
        return auctions;

    }

    public List<Auction> GetAuctionsToBid(string username)
    {
        List<Auction> auctions = new List<Auction>();
        List<Auction> auctionsStillinBid = new List<Auction>();

        auctions = _auctionPersistance.GetAuctionsToBid(username);
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
       return _auctionPersistance.GetAuctionsWon(username);
    }
    
    
    
}