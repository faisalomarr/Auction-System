using AutoMapper;
using ProjectApp.Core;

namespace ProjectApp.Persistence;

public class BidRepository : GenericRepository<BidDb>, IBidRepository
{
    private readonly AuctionDbContext auctionDbContext;
    private readonly IMapper mapper;


    public BidRepository(AuctionDbContext context, IMapper mapper) : base(context)
    {
        auctionDbContext = context;  
        this.mapper = mapper;
    }
    
    
}