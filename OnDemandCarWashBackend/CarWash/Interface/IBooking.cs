using System;
using CarWash.Models;
using CarWash.Models.DTOs.AddDTOs;
using CarWash.Models.DTOs.GetDTO;
namespace CarWash.Interface;

public interface IBooking
{
    //Task<Bookings> CreateBookingAsync(Bookings booking);
    Task<List<GetAvailableWasherDTO>> GetAvailableWasher(string city, DateTime serviceDate,string timeSlot);
    //Task<bool> ConfirmBookingAsync(BookingRequest confirmBookingDTO);
    Task<int> ConfirmBookingAsync(Bookings newBooking);
    Task<Payment> CreateCashPaymentAsync(int bookingId);

    Task<IEnumerable<GetWasherDTOs>>GetBookingsByWasherId(Guid userId);
    Task<IEnumerable<GetCustomerDTOs>> GetBookingsByCustomerId(Guid userId);

    Task<Bookings>UpdateBookingStatus(int bookingId,Bookings newmodel);
    Task<Bookings?> BookingConfirmationAsync(int bookingId);
    Task<int> TotalBookings();
    Task<IEnumerable<Bookings>>AllBookings();
    Task<BookingStatusDTO>GetBookingStats();

    Task<IEnumerable<Bookings>>GetRecentBookingsByWasherId(Guid washerId);

}
