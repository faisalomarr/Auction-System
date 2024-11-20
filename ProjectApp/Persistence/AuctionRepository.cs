using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Core;

namespace ProjectApp.Persistence;

public class AuctionRepository : GenericRepository<AuctionDb>, IAuctionRepository
{
    private readonly AuctionDbContext auctionDbContext;
    private readonly IMapper mapper;


    public AuctionRepository(AuctionDbContext context, IMapper mapper) : base(context)
    {
        auctionDbContext = context;  
        this.mapper = mapper;
    }

   public List<Auction> GetAuctions()
    {
        var auctionDbs = auctionDbContext.AuctionsDbs
            .Where(a => a.AuctionEndTime> DateTime.Now)
            .OrderBy(a => a.AuctionEndTime)
            .ToList();
        
        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }
        
        return result;
    }

    public List<Auction> GetAuctionsWhereBid(string username)
    {
        var auctionDbs = auctionDbContext.AuctionsDbs
            .Where(auction => auctionDbContext.BidDbs
                .Any(bid => bid.username == username && bid.AuctionId == auction.Id) && auction.AuctionEndTime > DateTime.Now)
            .Distinct() 
            .ToList();


        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }
        
        return result;
        
    }

    public void ChangeAuctionDescription(int auctionId, string newDescription, string username)
    {
        var auctionDb = auctionDbContext.AuctionsDbs
            .FirstOrDefault(a => a.Id == auctionId && a.username == username);

        if (auctionDb == null){
            throw new ArgumentException("Auction not found or username does not own the auction");
        }

        auctionDb.Description = newDescription;

        auctionDbContext.SaveChanges();
        
    }


    public Auction GetAuctionById(int id)
    {
        AuctionDb? auctionDb = auctionDbContext.AuctionsDbs
            .Where(a => a.Id == id && a.AuctionEndTime > DateTime.Now)
            .Include(a => a.BidDbs.OrderByDescending(bidDb => bidDb.Amount)) 
            .FirstOrDefault();

        if (auctionDb == null) 
            throw new DataException("Auction not found");

        Auction auction = mapper.Map<Auction>(auctionDb);
        foreach (BidDb bidDb in auctionDb.BidDbs)
        {
            Bid bid = mapper.Map<Bid>(bidDb);
            auction.AddBid(bid);
        }

        return auction;
    }


    public List<Auction> GetAuctionsOfUser(string username)
    {
        List<AuctionDb> auctionDbs = auctionDbContext.AuctionsDbs
            .Where(auction => auction.username == username) 
            .ToList();
        
        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }

        return result;
    }
    
    public List<Auction> GetAuctionsWon(string username)
    {
        var wonAuctions = auctionDbContext.AuctionsDbs
            .Where(a => a.AuctionEndTime < DateTime.Now &&
                        a.BidDbs.Any() &&
                        a.BidDbs.OrderByDescending(b => b.Amount).FirstOrDefault()!.username == username)
            .ToList();
        

        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in wonAuctions)
        {
            Auction auction = mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }

        return result;
    }
}