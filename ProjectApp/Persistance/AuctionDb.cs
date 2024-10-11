using System.ComponentModel.DataAnnotations;
using ProjectApp.Core;

namespace ProjectApp.Persistance;
using System.ComponentModel.DataAnnotations.Schema; // Required for DatabaseGeneratedOption


public class AuctionDb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    
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