using ProjectApp.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectApp.Models.Auctions;

public class AuctionDetailsVm
{
    [ScaffoldColumn(false)] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [Display(Name = "End date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime AuctionEndTime { get; set; }

    public List<BidVm> _bidsVMs { get; set; } = new();



    // Static method to map from Auction entity to AuctionDetailsVM
    public static AuctionDetailsVm FromAuction(Auction auction)
    {
        var detailsVM = new AuctionDetailsVm()
        {
            Id = auction.Id,
            Name = auction.Name,
            Description = auction.Description,
            AuctionEndTime = auction.AuctionEndTime,
        };
        foreach (var bid in auction.Bids)
        {
            detailsVM._bidsVMs.Add(BidVm.FromBid(bid));
        }

        return detailsVM;
    }
}