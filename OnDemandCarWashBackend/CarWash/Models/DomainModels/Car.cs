
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarWash.Models{
public class Car
{
    [Key]
    public int CarId { get; set; }
    //[ForeignKey("User")]
    public Guid UserId { get; set; } // Foreign Key to User
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Model { get; set; }
   
    public int Year { get; set; }
    [Required]
    public string LicensePlate { get; set; }
    
    public string CarImage { get; set; }
    

    // Navigation Property
    public User User { get; set; }

    public ICollection<Bookings>Bookings {get;set;}
}
}