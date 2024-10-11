namespace ProjectApp.Core.Interfaces;

public interface IAuctionPersistance
{
    List<Auction> GetAuctions();
    
    void AddAuction(Auction auction);
    void ChangeAuctionDescription(int auctionId, String description,string username);
}