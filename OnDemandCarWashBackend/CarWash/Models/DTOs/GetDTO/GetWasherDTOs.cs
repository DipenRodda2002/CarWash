using System;

namespace CarWash.Models.DTOs.GetDTO;

public class GetWasherDTOs
{
    public int BookingId{get;set;}
    public string Name{get;set;}

    public DateTime OrderDate { get; set; }
    public DateTime ServiceDate { get; set; }
     public decimal TotalPrice { get; set; }
     public string PaymentMethod { get; set; }
     public string OrderStatus{get;set;}


}
