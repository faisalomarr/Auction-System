namespace ProjectApp.Core;

using System;

public class Bid
{
    public int BidId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TimeOfBid { get; set; }
    
    public String username { get; set; }

    public Bid(decimal amount)
    {
        Amount = amount;
        TimeOfBid = DateTime.Now;
    }
    
    public Bid(int bidId, decimal amount, DateTime timeOfBid, String userName)
    {
        BidId = bidId;
        Amount = amount;
        TimeOfBid = timeOfBid;
        username = userName;
    }
    
    public Bid() { }

}
