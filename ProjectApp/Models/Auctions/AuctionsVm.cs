using System.ComponentModel.DataAnnotations;
using ProjectApp.Core;

namespace ProjectApp.Models.Auctions;

public class AuctionsVm
{   
    [ScaffoldColumn(false)] 
    public int Id { get; set; }
    public String Name { get; set; }
    
    public String Description { get; set; }
    
    public Decimal Price { get; set; }
    
    [Display(Name = "End date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime AuctionEndTime { get; set; }
    public String Username { get; set; }


    public static AuctionsVm FromAuction(Auction auction)
    {
        return new AuctionsVm()
        {   
            Id = auction.Id,
            Name = auction.Name,
            Description = auction.Description,
            Price = auction.Price,
            AuctionEndTime = auction.AuctionEndTime,
            Username = auction.Username
        };
    }
}