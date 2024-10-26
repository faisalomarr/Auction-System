namespace ProjectApp.Core.Interfaces;

public interface IAuctionPersistance
{
    List<Auction> GetAuctions();
    
    List<Auction> GetAuctionsWhereBid(string username);
    
    List<Auction> GetAuctionsWhereBidHighest(string username);
    
    void AddAuction(Auction auction);
    void ChangeAuctionDescription(int auctionId, String description,string username);
}