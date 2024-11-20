namespace ProjectApp.Core;

public class Auction : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    
    private DateTime _auctionEndTime;
    public DateTime AuctionEndTime
    {
        get => _auctionEndTime;
        set
        {
            // Ensure the end date is in the future
            if (value <= DateTime.Now)
            {
                _auctionEndTime = DateTime.Now.AddDays(1); // Set to one day forward if past
            }
            else
            {
                _auctionEndTime = value;
            }
        }
    }

    public string Username { get; set; }
    
    private List<Bid> _bids = new List<Bid>();
    public IEnumerable<Bid> Bids => _bids;

    
    public Auction() { }

    public Auction(string name, string description, decimal price, DateTime endDateTime, string username)
    {
        Name = name;
        Description = description;
        Price = price;
        AuctionEndTime = endDateTime; // Uses the set logic
        Username = username;
    }
    
    public void AddBid(Bid bid)
    {
        _bids.Add(bid);
    }
}