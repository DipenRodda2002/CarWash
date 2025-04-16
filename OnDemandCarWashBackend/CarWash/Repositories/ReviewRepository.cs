using System;
using CarWash.Data;
using CarWash.Interface;
using CarWash.Models.DTOs.GetDTO;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repositories;

public class ReviewRepository : IReview
{
    private readonly CarWashContext context;
    public ReviewRepository(CarWashContext ct){
        context = ct;

    }

    public async Task<IEnumerable<GetReviewsDTO>> GetReviewsForWasherAsync(Guid washerId)
    {
        var result =  await context.Reviews.Where(r => r.WasherId == washerId)
        .Select(r=> new GetReviewsDTO{
            Email = r.Customer.Email,
            Comment = r.Comment,
            Rating = r.Rating
        }).ToListAsync();
        return result;
    }

    public async Task<ReviewRating> GetReviewByBookingIdAsync(int bookingId)
    {
        var result=   await context.Reviews.FirstOrDefaultAsync(r => r.BookingId == bookingId);

        return  result;
    }

    public async Task<bool> AddReviewAsync(ReviewRating review)
    {
        try{
            if(await context.Reviews.AnyAsync(u=>u.BookingId==review.BookingId)){
                return false;
            }
        var res = await context.Bookings.SingleOrDefaultAsync(x=>x.BookingId==review.BookingId);
        review.WasherId= res.WasherId;
        
        context.Reviews.Add(review);
        await context.SaveChangesAsync();
        return true;
        }
        catch(Exception ex){
            Console.WriteLine("Error : "+ex);
            return false;
        }
    }
    // public async Task<int>AverageRatings(Guid  washerId){
    //     var res = context.Reviews.GroupBy(x=>x.Booking.WasherId==washerId);

    // }


}
