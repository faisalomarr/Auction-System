namespace ProjectApp.Core;

using System;

public class Bid
{
    
    public decimal Amount { get; set; }
    public DateTime TimeOfBid { get; set; }

    public Bid(decimal amount)
    {
        
        Amount = amount;
        TimeOfBid = DateTime.Now;
    }
}
