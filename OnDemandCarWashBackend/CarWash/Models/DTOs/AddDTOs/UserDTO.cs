using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashApp.Models.DTOs;

public class UserDTO
{
    public Guid UserId{get;set;}

    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Password { get; set; }
    public string? ProfileImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    [Required]
    public string Role{get;set;}
   

   
    
}
 
    public class UserResponseDto
    {
 
        public Guid UserId { get; set; } // Primary Key
        public string Name { get; set; }
 
        public string Email { get; set; }
        public string[] Role { get; set; } // "Admin" or "Doctor"
    }
