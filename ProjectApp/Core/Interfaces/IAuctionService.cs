namespace ProjectApp.Core.Interfaces;

public interface IAuctionService
{
    List<Auction> GetAuctions();
    
    List<Auction> GetAuctionsWhereBid(string username);
    
    void AddAuction(String username, String title , decimal price, DateTime endDate, string description);
    
    void ChangeAuctionDecription(int auctionId, string description,string username);
    
    Auction GetAuctionById(int auctionId);
    
    List<Auction> GetAuctionsOfUser(string username);
    
    List<Auction> GetAuctionsToBid(string username);
    
    public List<Auction> GetAuctionsWon(string username);
    
    void AddBid(string username, decimal bid,int auctionId);




    
}