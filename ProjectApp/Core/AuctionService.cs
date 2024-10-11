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
        auctions=_auctionPersistance.GetAuctions();
        return auctions;
    }

    public void AddAuction(Auction auction)
    {
        throw new NotImplementedException();
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
}
