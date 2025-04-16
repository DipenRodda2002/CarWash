
using CarWashApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarWash.Models
{
    public class Bookings
    {
        [Key]
        public int BookingId { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public User Customer { get; set; }
        public Guid WasherId { get; set; }
        [ForeignKey("WasherId")]
        public User Washer { get; set; }
        public int PackageId { get; set; } 
        [ForeignKey("PackageId")]    // Foreign Key to WashPackage
        public WashPackage WashPackage { get; set; }
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }        
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; }
        public Payment Payment { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime ServiceDate { get; set; }
        [Required]
        public string TimeSlot { get; set; }
        [Required]
        public string OrderStatus { get; set; }
        public string Notes { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public ICollection<AddOn> AddOns { get; set; }
        
    }
}

