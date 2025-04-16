using System;

namespace CarWash.Models.DTOs.AddDTOs;

public class PaymentRequestDTO
{

    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }

}
