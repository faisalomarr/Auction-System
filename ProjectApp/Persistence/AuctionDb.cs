using System.ComponentModel.DataAnnotations;
using ProjectApp.Core;

namespace ProjectApp.Persistence;
using System.ComponentModel.DataAnnotations.Schema; // Required for DatabaseGeneratedOption


public class AuctionDb : BaseEntity
{
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
    
    [Required]
    public decimal StartPrice { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime AuctionEndTime { get; set; }
    
    [Required]
    [MaxLength(100)] 
    public String Username { get; set; }
    
    public List<BidDb> BidDbs { get; set; } = new List<BidDb>();
    
    
}