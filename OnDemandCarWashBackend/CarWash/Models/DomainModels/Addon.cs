using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarWash.Models{
    public class AddOn
{
    [Key]
    public int AddOnId { get; set; }
    [Required]
    public string AddOnName { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }

    // Navigation Property
    
}
}