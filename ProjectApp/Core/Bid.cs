﻿namespace ProjectApp.Core;

using System;

public class Bid : BaseEntity
{
    public decimal Amount { get; set; }
    public DateTime TimeOfBid { get; set; }
    
    public String username { get; set; }
    
    public int AuctionId { get; set; }

    public Bid(decimal amount)
    {
        Amount = amount;
        TimeOfBid = DateTime.Now;
    }
    
    public Bid( decimal amount, DateTime timeOfBid, String userName, int auctionId)
    {
        Amount = amount;
        TimeOfBid = timeOfBid;
        username = userName;
        AuctionId = auctionId;
    }

}
