using CarWash.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CarWash.Models.DTOs;
using CarWash.Models.DTOs.AddDTOs;
using AutoMapper;
using System.Security.Claims;
using CarWash.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CarWash.Models.DTOs.GetDTO;
namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReview repo;
        private readonly IMapper mapper;
        private readonly CarWashContext context;
        public ReviewsController(IReview repository, IMapper _mapper,CarWashContext ct)
        {
            repo = repository;
            mapper = _mapper;
            context = ct;
        }

        [HttpPost("AddReview")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> GiveReviewToWasher(GiveReviewDTO reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {

                var newreview = new ReviewRating{
                    Rating=reviewDto.Rating,
                    Comment =reviewDto.Comment,
                    CustomerId = Guid.Parse(userIdClaim),
                    BookingId = reviewDto.BookingId
                };



                bool status = await repo.AddReviewAsync(newreview);
                if (status)
                {
                    return Ok(new { message = "Review submitted successfully" });
                }
                return BadRequest(new { message = "Failed to submit review" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }

        [HttpGet("Get-Review-Washer-Id/{WasherId}")]
        [Authorize(Roles ="Customer,Admin,Washer")]
        public async Task<IActionResult> GetReviewsByWasherId(Guid WasherId){
            var result = await repo.GetReviewsForWasherAsync(WasherId);

            
            if(result.Any()){
                var ReviewDTo = mapper.Map<IEnumerable<GetReviewsDTO>>(result);
                return Ok(ReviewDTo);
            }
            return NotFound("No Reviews Found for this Washer");
        }


    }
}
