using System;

namespace CarWash.Models.DTOs.AddDTOs;

public class BookingRequest
{
    public Guid WasherId { get; set; }
    public int PackageId { get; set; }
    public int AddressId { get; set; }
    public int CarId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string TimeSlot{get;set;}
    public string Notes { get; set; }
    public string PaymentMethod { get; set; }
    public decimal TotalPrice { get; set; }

}
