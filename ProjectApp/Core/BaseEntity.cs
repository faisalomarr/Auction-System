using System.ComponentModel.DataAnnotations;

namespace ProjectApp.Core;

public class BaseEntity
{
    [Key] 
    public int Id { get; set; }
}