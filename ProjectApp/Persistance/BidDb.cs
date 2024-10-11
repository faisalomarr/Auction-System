﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectApp.Core;

namespace ProjectApp.Persistance;

public class BidDb
{
    [Key]
    public int BidId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string username { get; set; }
    
    [Required]
    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime TimeOfBid { get; set; }
    
    [ForeignKey("AuctionId")]
    public AuctionDb Auction { get; set; }
    
    public int AuctionId { get; set; }

}