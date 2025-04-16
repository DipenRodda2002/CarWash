using System;
using System.ComponentModel.DataAnnotations;

namespace CarWash.Models.DTOs.GetDTO;

public class GetReviewsDTO
{
    // [Required]
    // public Guid  CustomerId {get; set;}
    public string Email{get;set;} 
   
    // [Required]
    // [StringLength(100, MinimumLength = 3)]
    public  string Comment {get; set;}
 
    // [Required]
    // [Range(1,5)]
    public int Rating {get; set;}

}
