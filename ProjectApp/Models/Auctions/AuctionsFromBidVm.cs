namespace ProjectApp.Models.Auctions;
using System.ComponentModel.DataAnnotations;
using ProjectApp.Core;

public class AuctionsFromBidVm
{
    public String Name { get; set; }
    
    public String Description { get; set; }
    
    public Decimal Price { get; set; }
    
    [Display(Name = "End date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime AuctionEndTime { get; set; }


    public static AuctionsFromBidVm FromAuction(Auction auction)
    {
        return new AuctionsFromBidVm()
        {
            Name = auction.Name,
            Description = auction.Description,
            Price = auction.StartPrice,
            AuctionEndTime = auction.AuctionEndTime
        };
    }
}