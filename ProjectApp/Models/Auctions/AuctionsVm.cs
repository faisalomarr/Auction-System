using System.ComponentModel.DataAnnotations;
using ProjectApp.Core;

namespace ProjectApp.Models.Auctions;

public class AuctionsVm
{
    public String Name { get; set; }
    
    public String Description { get; set; }
    
    public Decimal Price { get; set; }
    
    [Display(Name = "End date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime AuctionEndTime { get; set; }


    public static AuctionsVm FromAuction(Auction auction)
    {
        return new AuctionsVm()
        {
            Name = auction.Name,
            Description = auction.Description,
            Price = auction.StartPrice,
            AuctionEndTime = auction.AuctionEndTime
        };
    }
}