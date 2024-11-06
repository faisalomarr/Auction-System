using ProjectApp.Core;

namespace ProjectApp.Models.Bids;
using System.ComponentModel.DataAnnotations;

public class BidVm
{
    public decimal Amount { get; set; }
    
    
    [Display(Name = "Time of Bid")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime TimeOfBid { get; set; }

    public static BidVm FromBid(Bid bid)
    {
        return new BidVm()
        {
            Amount = bid.Amount,
            TimeOfBid = bid.TimeOfBid,
        };
    }
}