namespace ProjectApp.Models.Bids;

public class AddBidVm
{
    
    public int AuctionId { get; set; }

    public decimal Price { get; set; } // Starting price of the auction

    public DateTime EndDate { get; set; } // End date and time for the auction

}