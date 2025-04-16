using System;
using System.ComponentModel.DataAnnotations;

namespace CarWash.Models.DTOs.AddDTOs;

public class UpdateUserDTO
{
    
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string? ProfileImage { get; set; }
    public bool IsActive { get; set; }

}
