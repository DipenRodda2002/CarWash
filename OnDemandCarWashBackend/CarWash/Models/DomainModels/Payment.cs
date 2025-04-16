using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarWash.Models
{


    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int BookingId { get; set; } // Foreign Key to Order
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public decimal PaymentAmount { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string PaymentMethod { get; set; }

        // Navigation Property
         [ForeignKey("BookingId")]
        public Bookings Bookings { get; set; }
        
        
    }
}


//addressscusto