using System;
using CarWash.Models;

namespace CarWash.Interface;

public interface IEmailService
{
    string GenerateBookingEmail(Bookings booking);
    Task SendBookingRequestAsync(string washerEmail, Bookings booking);

}
