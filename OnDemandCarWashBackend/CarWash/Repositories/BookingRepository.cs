
using System;
using CarWash.Models;
using CarWash.Data;
using Microsoft.EntityFrameworkCore;
using CarWash.Interface;
using CarWash.Models.DTOs.AddDTOs;
using System.Security.Claims;
using System.Text.Json;
using CarWashBookingSystem.Repositories;
using CarWash.Models.DTOs.GetDTO;

namespace CarWash.Repositories;

public class BookingRepository : IBooking
{
    private readonly CarWashContext context;
    private readonly IEmailService _emailService;
    public BookingRepository(CarWashContext ct, IEmailService emailService)
    {
        context = ct;
        _emailService = emailService;

    }

    // public async Task<List<User>> GetAvailableWasher(string city, DateTime serviceDate, string timeSlot)
    // {
    //     var washersInCity = await context.Users
    //     .Where(w => w.Roles == "Washer" && w.Addresses.Any(c => c.City == city)).ToListAsync();

    //     // var availableWashers = washersInCity
    //     // .Where(w => context.Bookings
    //     //     .Count(b => b.WasherId == w.UserId && b.ServiceDate.Date == serviceDate.Date) < 5)
    //     // .ToList();
    //     var availableWashers = washersInCity
    //     .Where(w => !context.Bookings
    //         .Any(b => b.WasherId == w.UserId &&
    //                   b.ServiceDate.Date == serviceDate.Date &&
    //                   b.TimeSlot == timeSlot)) // Compare by time slot
    //     .ToList();

    //     return availableWashers;
    // }
    public async Task<List<GetAvailableWasherDTO>> GetAvailableWasher(string city, DateTime serviceDate, string timeSlot)
    {
        // Step 1: Get washers in city
        var washersInCity = await context.Users
            .Where(w => w.Roles == "Washer" && w.Addresses.Any(c => c.City == city)).Include(w => w.Addresses)
            .ToListAsync();

        // Step 2: Filter available washers by date and time slot
        var availableWashers = washersInCity
            .Where(w => !context.Bookings
                .Any(b => b.WasherId == w.UserId &&
                          b.ServiceDate.Date == serviceDate.Date &&
                          b.TimeSlot == timeSlot))
            .ToList();

        // Step 3: Get average ratings
        var washerRatings = await context.Reviews
            .GroupBy(r => r.WasherId)
            .Select(g => new
            {
                WasherId = g.Key,
                AverageRating = g.Average(r => r.Rating),
                TotalReviews = g.Count()
            })
            .ToListAsync();
        var washerDtos = availableWashers.Select(w =>
        {
            var ratingInfo = washerRatings.FirstOrDefault(r => r.WasherId == w.UserId);

            return new GetAvailableWasherDTO
            {
                WasherId = w.UserId,
                Name = w.Name,
                City = w.Addresses?.FirstOrDefault()?.City ?? "N/A",
                AverageRating = ratingInfo?.AverageRating ?? 0,
                TotalReviews = ratingInfo?.TotalReviews ?? 0
            };
        })
.OrderByDescending(w => w.AverageRating)
.ToList();

        return washerDtos;
    }


    public async Task<int> ConfirmBookingAsync(Bookings newBooking)
    {
        var package = await context.WashPackages.FirstOrDefaultAsync(p => p.PackageId == newBooking.PackageId);
        if (package == null)
            return 0;

        newBooking.OrderDate = DateTime.Now;
        newBooking.OrderStatus = "Pending";
        newBooking.TotalPrice = package.Price;
        newBooking.PaymentStatus = "Pending";


        await context.Bookings.AddAsync(newBooking);
        await context.SaveChangesAsync();
        // if (newBooking.PaymentMethod.ToLower() == "cash")
        // {
        //     await ConfirmCashBookingAsync(newBooking.BookingId);
        // }



        var washer = await context.Users.FirstOrDefaultAsync(w => w.UserId == newBooking.WasherId);
        if (washer == null || string.IsNullOrEmpty(washer.Email))
        {
            Console.WriteLine("Error: Washer details are missing or email is null.");
            return 0;
        }

        var book = await context.Bookings.Include(x => x.Customer).Include(x => x.Address)
        .Include(x => x.Car).Include(x => x.WashPackage).
        FirstOrDefaultAsync(x => x.BookingId == newBooking.BookingId);


        await _emailService.SendBookingRequestAsync(newBooking.Washer.Email, book);


        return newBooking.BookingId;
    }

    public async Task<IEnumerable<GetWasherDTOs>> GetBookingsByWasherId(Guid userId)
    {
        var result = await context.Bookings.Where(x => x.WasherId == userId).OrderByDescending(r => r.OrderDate).Select(r => new GetWasherDTOs
        {
            Name = r.Customer.Name,
            BookingId = r.BookingId,
            OrderDate = r.OrderDate,
            ServiceDate = r.ServiceDate,
            TotalPrice = r.TotalPrice,
            PaymentMethod = r.PaymentMethod,
            OrderStatus = r.OrderStatus




        }).ToListAsync();
        if (result.Any())
        {
            return result;
        }
        return null;
    }

    public async Task<Bookings> UpdateBookingStatus(int bookingId, Bookings newmodel)
    {
        var existingbooking = await context.Bookings.SingleOrDefaultAsync(x => x.BookingId == bookingId);
        if (existingbooking != null)
        {
            existingbooking.OrderStatus = newmodel.OrderStatus;
            await context.SaveChangesAsync();
            return newmodel;

        }
        return null;
    }

    public async Task<Payment> CreateCashPaymentAsync(int bookingId)
    {
        // var booking = await context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
        // if (booking == null) return null;

        // booking.OrderStatus = "Confirmed";
        // booking.PaymentStatus = "Not Required"; // No online payment needed

        // await context.SaveChangesAsync();
        // return booking;
        var booking = await context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
        if (booking == null|| booking.OrderStatus == "Confirmed") return null;

        // Mark booking status
        booking.OrderStatus = "Confirmed";
        booking.PaymentStatus = "Pending";

        // 💵 Add payment entry for cash
        var payment = new Payment
        {
            BookingId = bookingId,
            PaymentAmount = booking.TotalPrice, // assuming this exists
            PaymentMethod = "Cash",
            PaymentDate = DateTime.Now,
            Status = "Pending"
        };

        context.Payments.Add(payment);

        await context.SaveChangesAsync();
        return payment;
    }

    public async Task<Bookings?> BookingConfirmationAsync(int bookingId)
    {
        return await context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Washer)
            .Include(b => b.Address)
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }
    public async Task<IEnumerable<GetCustomerDTOs>> GetBookingsByCustomerId(Guid userId)
    {
        var result = await context.Bookings.Where(x => x.CustomerId == userId).OrderByDescending(r => r.OrderDate).Select(r => new GetCustomerDTOs
        {
            Name = r.Washer.Name,
            BookingId = r.BookingId,
            OrderDate = r.OrderDate,
            ServiceDate = r.ServiceDate,
            TotalPrice = r.TotalPrice,
            PaymentMethod = r.PaymentMethod,
            OrderStatus = r.OrderStatus




        }).ToListAsync();
        if (result.Any())
        {
            return result;
        }
        return null;
    }

    public async Task<int> TotalBookings()
    {
        var result = await context.Bookings.CountAsync(x => x.OrderStatus == "Pending" || x.OrderStatus == "Confirmed" || x.OrderStatus == "Completed" || x.OrderStatus == "Cancelled");
        if (result != 0)
        {
            return result;
        }
        return 0;
    }
    public async Task<IEnumerable<Bookings>> AllBookings()
    {
        var result = await context.Bookings.OrderByDescending(x => x.BookingId).Include(b => b.Customer)
            .Include(b => b.Washer)
            .Include(b => b.Address)
            .Include(b => b.WashPackage)
            .Include(b => b.Customer)
            .Include(b => b.Car)
            .ToListAsync();
        if (result.Any())
        {
            return result;
        }
        return new List<Bookings>();
    }

    public async Task<IEnumerable<Bookings>>GetRecentBookingsByWasherId(Guid washerId){
        
        var res = await context.Bookings.Where(x=>x.WasherId==washerId)
        .OrderByDescending(x=>x.BookingId)
        .Include(x=>x.Address)
        .Include(x=>x.Car)
        .Include(x=>x.WashPackage)
        .Include(x=>x.Customer)
        .Include(x=>x.Payment)
        .Take(3).ToListAsync();
        if(res.Any()){
            return res;
        }
        return new List<Bookings>();
    }

    public async Task<BookingStatusDTO>GetBookingStats(){
        var today = DateTime.Today;
        var past7Days = today.AddDays(-6);
        var booking = await context.Bookings.Where(x=>x.OrderDate>=past7Days).ToListAsync();
        var dailytrends = booking.GroupBy(b=>b.OrderDate)
                            .Select(g=> new DailyBookingTrendDto{
                                Date=g.Key.ToString("MM-dd"),
                                Count = g.Count()
                            })
                            .OrderBy(d=>d.Date)
                            .ToList();

        var stats = new BookingStatusDTO
        {
            TotalBookings = booking.Count,
            CompletedBookings = booking.Count(b => b.OrderStatus == "Completed"),
            PendingBookings = booking.Count(b => b.OrderStatus == "Pending"),
            CancelledBookings = booking.Count(b => b.OrderStatus == "Cancelled"),
            DailyTrends = dailytrends
        };
         return stats;
    }








}
