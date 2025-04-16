using System;

namespace CarWash.Models.DTOs.AddDTOs;

public class BookingResponseDTO
{
    public Guid BookingId { get; set; }
    public Guid WasherId { get; set; }
    public string WasherName { get; set; }
    public DateTime ServiceDate { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    
}
