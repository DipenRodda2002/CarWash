using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWash.Models;

public class Address
{
    [Key]
    public int AddressId { get; set; }
    public Guid UserId { get; set; } 
    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string State { get; set; }
    [Required]
    public string Pincode { get; set; }
    [Required]
    public string AddressType { get; set; } 
    public bool IsActive { get; set; } = true;
    // Navigation Property
    public User User { get; set; } // User associated with this address
    // public Order Order { get; set; } // Order associated with this address


}
// public int OrderId { get; set; } // Foreign Key to Order (if this address belongs to an order)