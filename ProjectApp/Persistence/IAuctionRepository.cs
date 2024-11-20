using ProjectApp.Core;

namespace ProjectApp.Persistence;

public interface IAuctionRepository : IGenericRepository<AuctionDb>
{
    List<Auction> GetAuctions();
    
    List<Auction> GetAuctionsWhereBid(string username); 
    
    void ChangeAuctionDescription(int auctionId, String description,string username);
    
    Auction GetAuctionById(int auctionId);
    
    List<Auction> GetAuctionsOfUser(string username);
    
    List<Auction> GetAuctionsWon(string username);
    
}