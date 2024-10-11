namespace ProjectApp.Core.Interfaces;

public interface IAuctionPersistance
{
    List<Auction> GetAuctions();
    
    void AddAuction(Auction auction);
}