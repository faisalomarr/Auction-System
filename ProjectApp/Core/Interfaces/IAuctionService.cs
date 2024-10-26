namespace ProjectApp.Core.Interfaces;

public interface IAuctionService
{
    List<Auction> GetAuctions();
    
    List<Auction> GetAuctionsWhereBid(string username);
    
    void AddAuction(String username, String title , decimal price, DateTime endDate);
    
    void ChangeAuctionDecription(int auctionId, string description,string username);
}