// using Microsoft.EntityFrameworkCore;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// namespace CarWash.Models
// {


//     public class Review
//     {
//         [Key]
//         public int ReviewId { get; set; }
//         public int UserId { get; set; } // Foreign Key to User
//                                         // public int WasherId { get; set; } // Foreign Key to Washer
//         public int BookingId { get; set; } // Foreign Key to Order

//         public int Rating { get; set; } // 1 to 5 scale
//         public string ReviewText { get; set; }
//         public DateTime CreatedAt { get; set; }

//         // Navigation Property
//         public User User { get; set; }
//         // public Washer Washer { get; set; }
//         public Bookings Bookings { get; set; }
//     }
// }