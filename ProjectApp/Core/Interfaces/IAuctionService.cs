namespace ProjectApp.Core.Interfaces;

public interface IAuctionService
{
    List<Auction> GetAuctions();
    
    void AddAuction(String username, String title , decimal price, DateTime endDate);
}