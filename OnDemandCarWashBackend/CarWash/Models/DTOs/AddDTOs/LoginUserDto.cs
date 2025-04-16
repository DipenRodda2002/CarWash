using System;
using System.ComponentModel.DataAnnotations;

namespace CarWash.Models.DTOs.AddDTOs;

public class LoginUserDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format. Please enter a valid email address.")]
        public string Username { get; set; }
 
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
