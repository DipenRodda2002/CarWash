using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarWash.Models;

public class ReviewRating
{
    [Key]
    public int ReviewId { get; set; }

    [Required]
    public int BookingId { get; set; } 

    [Required]
    public Guid CustomerId { get; set; }  

    [Required]
    public Guid WasherId { get; set; }  

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }  

    [MaxLength(500)]
    public string Comment { get; set; }  

    [Required]
    public DateTime ReviewDate { get; set; } = DateTime.Now;  

    // Navigation Properties
    [ForeignKey("BookingId")]
    public Bookings Booking { get; set; }

    [ForeignKey("CustomerId")]
    public User Customer { get; set; }

    [ForeignKey("WasherId")]
    public User Washer { get; set; }
}
