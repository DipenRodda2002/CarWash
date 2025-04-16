using System;

namespace CarWash.Models.DTOs.AddDTOs;

public class PaymentDTO
{
    public int BookingId { get; set; }
    public decimal PaymentAmount { get; set; }
    public string PaymentMethod { get; set; }

}
