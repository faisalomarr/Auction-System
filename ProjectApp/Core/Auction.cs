namespace ProjectApp.Core;

public class Auction
{
    public int Id { get; set; } // Ensure this property is present
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal StartPrice { get; set; }
    public DateTime AuctionEndTime { get; set; }
    
    public string Username { get; set; }
    
    
    
    private List<Bid> _bids = new List<Bid>();
    public IEnumerable<Bid> Bids => _bids;
    
    
    public Auction(int id ,string name, string description, decimal startPrice, DateTime endDateTime, String username)
    {
        Id = id;
        Name = name;
        Description = description;
        StartPrice = startPrice;
        AuctionEndTime = endDateTime;
        Username = username;
    }


    public void AddBid(Bid bid)
    {
        _bids.Add(bid);
    }
}