using System;

namespace CarWash.Models.DTOs.GetDTO;

public class BookingDetailsDto
{
    public int BookingId { get; set; }
    public string CustomerName { get; set; }
    public string WasherName { get; set; }
    public string City { get; set; }
    public DateTime ServiceDate { get; set; }
    public string TimeSlot { get; set; }
    public string Status { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentId { get; set; }
    public decimal TotalPrice { get; set; }

}
