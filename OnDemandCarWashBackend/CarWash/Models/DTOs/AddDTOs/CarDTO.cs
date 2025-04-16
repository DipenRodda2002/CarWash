using System;
using System.ComponentModel.DataAnnotations;

namespace CarWashApp.Models.DTOs;

public class CarDTO
{

    [Required(ErrorMessage = "Brand is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Brand must be between 2 and 50 characters.")]
    public string Brand { get; set; }

    [Required(ErrorMessage = "Model is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Model must be between 1 and 50 characters.")]
    public string Model { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
    public int Year { get; set; }

    public string LicensePlate { get; set; }
    public IFormFile CarImage { get; set; } 


}
