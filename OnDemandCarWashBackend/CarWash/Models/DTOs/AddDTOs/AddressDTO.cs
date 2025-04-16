using System;
using System.ComponentModel.DataAnnotations;

namespace CarWash.Models.DTOs;

public class AddressDTO
{
    [Required(ErrorMessage = "Street is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Street must be between 3 and 100 characters.")]
    public string Street { get; set; }
    [Required(ErrorMessage = "City is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be between 2 and 50 characters.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Pincode is required.")]
    public string Pincode { get; set; }

    [Required(ErrorMessage = "State is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "State must be between 2 and 50 characters.")]
    public string State { get; set; }
    public string AddressType { get; set; }


}
