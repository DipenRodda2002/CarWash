using System;

namespace CarWash.Models.DTOs.GetDTO;

public class GetAllBookingsDTO
{
    public int BookingId { get; set; }
    public string CustomerName{get;set;}
    public string WasherName{get;set;}
    public string PackageName{get;set;}
    public string AddressName{get;set;}
    public string CarName{get;set;}

    public DateTime OrderDate { get; set; }
    public DateTime ServiceDate { get; set; }
    public string TimeSlot { get; set; }

    public string OrderStatus { get; set; }
    public string Notes { get; set; }

    public decimal TotalPrice { get; set; }

    public string PaymentStatus { get; set; }

    public string PaymentMethod { get; set; }

}
