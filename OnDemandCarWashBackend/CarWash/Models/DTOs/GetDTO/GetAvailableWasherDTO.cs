using System;

namespace CarWash.Models.DTOs.GetDTO;

public class GetAvailableWasherDTO
{
    public Guid WasherId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }

}
