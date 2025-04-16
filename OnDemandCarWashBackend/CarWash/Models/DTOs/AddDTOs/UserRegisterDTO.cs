using System;
using System.ComponentModel.DataAnnotations;

namespace CarWash.Models.DTOs.AddDTOs;

public class UserRegisterDTO
{
    [Required]
    public string Username{get;set;}

    [Required]
    public string Password{get;set;}
    [Required]
    public string[] Roles{get;set;}
    [Required]
    public string Name{get;set;}

    [Required]
    public string Phone { get; set; }

    public string? ProfileImage { get; set; }
    


}
