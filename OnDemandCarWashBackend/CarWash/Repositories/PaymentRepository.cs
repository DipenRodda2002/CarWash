using System;
using CarWash.Data;
using CarWash.Interface;
using CarWash.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repositories;

public class PaymentRepository : IPayment
{
    private readonly CarWashContext context;
    public PaymentRepository(CarWashContext ct)
    {
        context = ct;
    }
    public async Task<bool> ProcessPaymentAsync(int bookingId, decimal amount, string paymentMethod)
    {
        var booking = await context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
        if (booking == null) return false;

        if (amount != booking.TotalPrice) return false;

        var payment = new Payment
        {
            BookingId = booking.BookingId,
            PaymentDate = DateTime.Now,
            PaymentAmount = amount,
            Status = paymentMethod.ToLower() == "cash" ? "Pending Confirmation" : "Completed",
            PaymentMethod = paymentMethod
        };

        await context.Payments.AddAsync(payment);
        booking.OrderStatus = "Confirmed";
        booking.PaymentStatus = paymentMethod.ToLower() == "cash" ? "Pending Confirmation" : "Completed";

        await context.SaveChangesAsync();
        return true;
    }


}
