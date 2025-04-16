using System;
using CarWash.Models;

namespace CarWash.Interface;

public interface IPayment
{
    Task<bool> ProcessPaymentAsync(int bookingId, decimal amount, string paymentMethod);

}
