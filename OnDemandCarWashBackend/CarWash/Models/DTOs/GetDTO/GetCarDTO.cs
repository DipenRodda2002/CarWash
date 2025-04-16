using System;

namespace CarWash.Models.DTOs.GetDTO;

public class GetCarDTO
{
    public int CarId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string CarImage { get; set; }
}

