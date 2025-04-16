using System;

namespace CarWash.Models.DTOs.GetDTO;

public class WasherDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ProfileImage { get; set; }

}
