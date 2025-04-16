using System;

// using CarWash.Models.DTOs;
using CarWash.Models.DTOs.GetDTO;
namespace CarWash.Interface;

public interface IReview
{
    Task<IEnumerable<GetReviewsDTO>> GetReviewsForWasherAsync(Guid washerId);
    Task<ReviewRating> GetReviewByBookingIdAsync(int bookingId);
    Task<bool> AddReviewAsync(ReviewRating review);
}
