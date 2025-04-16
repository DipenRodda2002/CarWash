using System;

namespace CarWash.Models.DTOs.GetDTO;

public class GetAddressDTOs
{
    public int AddressId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Pincode { get; set; }
    public string AddressType { get; set; }
    public bool IsActive { get; set; }
}
