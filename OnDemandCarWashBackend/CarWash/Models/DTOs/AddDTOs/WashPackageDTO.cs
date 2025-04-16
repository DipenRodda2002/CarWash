using System;

namespace CarWash.Models.DTOs.AddDTOs;

public class WashPackageDTO
{
    public string PackageName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Duration { get; set; }
    
}

