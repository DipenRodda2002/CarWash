using System;

namespace CarWash.Models.DTOs.GetDTO;

public class GetAllUserDTO
{
    
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? ProfileImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public string Role { get; set; }

    public ICollection<Car>? Cars { get; set; }
    public ICollection<Address>?Addresses{get;set;}


}
