using System;

namespace CarWash.Models.DTOs.GetDTO;

public class GetPackagesDTO
{

    public int PackageId { get; set; }

    public string PackageName { get; set; }

    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Duration { get; set; }

}
