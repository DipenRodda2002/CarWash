using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CarWash.Models;


public class User
{
    [Key]
    public Guid UserId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Password { get; set; }

    // [Required]
    // public string DefaultAddress { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }  = true;


    
    [Required]
    public string Roles{get;set;}
    
    // Navigation Property
    public ICollection<Car> Cars { get; set; }
    public ICollection<Bookings> Bookings { get; set; }
    //public ICollection<Review> Reviews { get; set; }
    public ICollection<Address> Addresses { get; set; }

    internal static string FindFirst(string role)
    {
        throw new NotImplementedException();
    }

    internal static object FindFirstValue(string nameIdentifier)
    {
        throw new NotImplementedException();
    }

    // public ICollection<Review> CustomerReviews { get; set; } 
    // public ICollection<Review> WasherReviews { get; set; } 
}

