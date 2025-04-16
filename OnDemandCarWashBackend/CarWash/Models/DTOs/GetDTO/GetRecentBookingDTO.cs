using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CarWash.Models.DTOs.GetDTO;

public class GetRecentBookingDTO
{

    public string CustomerName{get;set;}
    public string Model{get;set;}
    public DateTime OrderDate{get;set;}
    public DateTime ServiceDate{get;set;}
    public string PackageName{get;set;}
    public string OrderStatus{get;set;} 


}
