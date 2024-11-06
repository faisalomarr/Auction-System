namespace ProjectApp.Models.Auctions;

public class AddAuctionVm
{

    public string Title { get; set; } = string.Empty; // Title of the auction

    public decimal Price { get; set; } // Starting price of the auction

    public DateTime EndDate { get; set; } // End date and time for the auction
    
    public string Description { get; set; } = string.Empty; // Description for the auction

}
