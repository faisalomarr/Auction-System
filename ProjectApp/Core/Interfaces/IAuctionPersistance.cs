namespace ProjectApp.Core.Interfaces;

public interface IAuctionPersistance
{
    List<Auction> GetAuctions();
    
    List<Auction> GetAuctionsWhereBid(string username);
    
    void AddAuction(Auction auction);
    void ChangeAuctionDescription(int auctionId, String description,string username);
    
    Auction GetAuctionById(int auctionId);
    
    List<Auction> GetAuctionsOfUser(string username);
    
    List<Auction> GetAuctionsToBid(string username);
    public List<Auction> GetAuctionsWon(string username);


}