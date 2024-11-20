using AutoMapper;
using ProjectApp.Core.Interfaces;
using ProjectApp.Persistence;

namespace ProjectApp.Core;

public class AuctionService : IAuctionService
{
    
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IMapper mapper;


    public AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository, IMapper mapper )
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
        this.mapper=mapper;
    }
    
    public List<Auction> GetAuctions()
    {
        List<Auction> auctions=_auctionRepository.GetAuctions();
        return auctions;
    }

    public List<Auction> GetAuctionsWhereBid(string username)
    {
        List<Auction> auctions = _auctionRepository.GetAuctionsWhereBid(username);
        return auctions;

    }
    

    public void AddAuction(String username, String title , decimal price, DateTime endDate, String description)
    {
        if (username == null || title == null || title == "")
        {
            throw new ArgumentNullException();
        }
        Auction auction = new Auction(title,description,price,endDate,username);
        AuctionDb auctionDb = mapper.Map<AuctionDb>(auction);
        
        _auctionRepository.Add(auctionDb);
    }

    public void ChangeAuctionDecription(int auctionId, string description, String username)
    {
        if (auctionId < 1 || description == null || username == null)
        {
            throw new ArgumentNullException();
        }
        
        _auctionRepository.ChangeAuctionDescription(auctionId, description,username);
    }

    public Auction GetAuctionById(int auctionId)
    {
        
        Auction auction = _auctionRepository.GetAuctionById(auctionId);
        if (auction == null)
        {
            throw new ArgumentException("Auction not found");
        }

        return auction;
    }

    public List<Auction> GetAuctionsOfUser(string username)
    {
        List<Auction> auctions = _auctionRepository.GetAuctionsOfUser(username);
        return auctions;

    }

    public List<Auction> GetAuctionsWon(string username)
    {
        if (username == null)
        {
            throw new ArgumentNullException();
        } 
        return _auctionRepository.GetAuctionsWon(username);
    }

    public void AddBid(string username, decimal bid, int auctionId)
    {
        AuctionDb auction = _auctionRepository.GetById(auctionId);

        if (auction == null || auction.username == username || auction.AuctionEndTime < DateTime.Now )
        {
            throw new Exception("Auction not found");
        }

        if (auction.Price > bid)
        {
            throw new ArgumentException("Bid invalid");
        }

        Bid userBid = new Bid(bid, DateTime.Now,username,auctionId );
        BidDb userBidDb = mapper.Map<BidDb>(userBid);

        _bidRepository.Add(userBidDb);
    }
    
    
}