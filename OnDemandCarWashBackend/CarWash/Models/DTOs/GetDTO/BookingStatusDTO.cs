using System;

namespace CarWash.Models.DTOs.GetDTO;

public class BookingStatusDTO
{
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int PendingBookings { get; set; }
    public int CancelledBookings { get; set; }
    public List<DailyBookingTrendDto> DailyTrends { get; set; }

}
