using System;
using System.ComponentModel.DataAnnotations;

namespace CarWash.Models.DTOs.AddDTOs;

public class GiveReviewDTO
{
    // [Required]
    // public Guid WasherId {get; set;}
 
    [Required]
    public int BookingId {get; set;}

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }  

    [MaxLength(500)]
    public string Comment { get; set; }  
}