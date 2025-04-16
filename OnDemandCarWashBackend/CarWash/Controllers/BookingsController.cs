using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarWash.Interface;
using CarWash.Models.DTOs.AddDTOs;
using CarWash.Models;
using CarWash.Data;
using Microsoft.EntityFrameworkCore;
using CarWash.Models.DTOs.GetDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBooking repo;
        private readonly IMapper mapper;
        private readonly CarWashContext context;
        public BookingsController(IBooking repository, CarWashContext ct, IMapper mappers)
        {
            repo = repository;
            context = ct;
            mapper = mappers;

        }

        [HttpGet("get-available-washers/{addressId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAvailableWashers([FromRoute] int addressId, DateTime ServiceDate, string timeSlot)
        {
            try
            {
                if (addressId <= 0)
                {
                    return BadRequest("Invalid address ID.");
                }

                if (ServiceDate.Date < DateTime.UtcNow.Date)
                {
                    return BadRequest("Service date cannot be in the past.");
                }

                var customerAddress = await context.Addresses.FirstOrDefaultAsync(a => a.AddressId == addressId);
                if (customerAddress == null)
                    return BadRequest("Invalid Address");

                string city = customerAddress.City;
                Console.WriteLine(city);


                var availableWashers = await repo.GetAvailableWasher(city, ServiceDate, timeSlot);

                if (!availableWashers.Any())
                    return BadRequest("No washers available for the selected date.");

                var washerDTOs = mapper.Map<IEnumerable<GetAvailableWasherDTO>>(availableWashers);
                return Ok(washerDTOs);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("confirm-booking")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConfirmBooking([FromBody] BookingRequest confirmBookingDTO)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                    return Unauthorized("User not authenticated");
                if (confirmBookingDTO == null)
                {
                    return BadRequest("Booking details are required.");
                }

                var package = await context.WashPackages.SingleOrDefaultAsync(x => x.PackageId == confirmBookingDTO.PackageId);

                var booking = mapper.Map<Bookings>(confirmBookingDTO);
                booking.CustomerId = Guid.Parse(userId);
                int bookingId = await repo.ConfirmBookingAsync(booking);
                decimal TotalPrice = package.Price;
                if (bookingId == 0)
                    return BadRequest("Invalid package");



                return Ok(new { totalPrice = TotalPrice, bookingId, message = "Booking Confirmed!" });

            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetBookingsByWasherId")]
        [Authorize(Roles = "Customer,Washer")]
        public async Task<ActionResult> GetBookingsByWasherId(Guid userId)
        {
            var result = await repo.GetBookingsByWasherId(userId);
            if (result.Any())
            {
                var getbookingdto = mapper.Map<IEnumerable<GetWasherDTOs>>(result);
                return Ok(getbookingdto);
            }
            return NotFound(new { message = "No Bookings Found" });
        }
        [HttpGet("GetBookingsByCustomerId")]
        // [Authorize(Roles = "Customer,Washer")]
        public async Task<ActionResult> GetBookingsByCustomerId(Guid userId)
        {
            var result = await repo.GetBookingsByCustomerId(userId);
            if (result.Any())
            {
                var getbookingdto = mapper.Map<IEnumerable<GetCustomerDTOs>>(result);
                return Ok(getbookingdto);
            }
            return NotFound(new { message = "No Bookings Found" });
        }

        [HttpPut("UpdateBookingStatus/{bookingId}")]
        public async Task<IActionResult> UpdateBooking(int bookingId, [FromBody]UpdateBookingStatusDTO newmodel)
        {
            var res = mapper.Map<Bookings>(newmodel);
            var result = await repo.UpdateBookingStatus(bookingId, res);
            if (result != null)
            {
                return Ok(new { message = "Booking Status Updated" });
            }
            return BadRequest(new { message = "Bad Request" });
        }
        [HttpPost("CreateCashPayment/{bookingId}")]
        public async Task<IActionResult> CreateCashPayment(int bookingId)
        {
            var payment = await repo.CreateCashPaymentAsync(bookingId);

            if (payment == null)
            {
                return NotFound(new { message = "Booking not found or already confirmed." });
            }

            return Ok(new
            {
                message = "Cash payment recorded and booking confirmed.",
                paymentId = payment.PaymentId
            });
        }

        [HttpGet("GetBookingsById/{bookingId}")]
        [Authorize(Roles = "Customer,Washer,Admin")]
        public async Task<IActionResult> GetBookingByBookingId(int bookingId)
        {
            var booking = await repo.BookingConfirmationAsync(bookingId);

            if (booking == null)
                return NotFound(new { message = "No Booking Found" });

            var bookingDto = mapper.Map<BookingDetailsDto>(booking);
            return Ok(bookingDto);
        }

        [HttpGet("TotalBookingsCOunt")]
        public async Task<IActionResult> TotalBookingCount()
        {
            var result = await repo.TotalBookings();
            if (result != 0)
            {
                return Ok(new { totalBookings = result });
            }
            return NotFound(new { totalBookings = result });
        }

        [HttpGet("AllBookings")]
        public async Task<IActionResult> AllBookings()
        {
            var result = await repo.AllBookings();

            if (result.Any())
            {
                var allBookingsDTO = mapper.Map<IEnumerable<GetAllBookingsDTO>>(result);

                return Ok(allBookingsDTO);

            }
            return NotFound(new { message = "No Bookings Found" });
        }
        [HttpGet("get-washer-recent-bookings")]
        public async Task<IActionResult>GetRecentWasherBookings(Guid washerId){
            var result = await repo.GetRecentBookingsByWasherId(washerId);
            if(result.Any()){
                var recentdto = mapper.Map<IEnumerable<GetRecentBookingDTO>>(result);
                return Ok(recentdto);
            }
            return NotFound(new {message = "No Recent Bookings Found"});
        }

        [HttpGet("booking-stats")]
        public async Task<IActionResult> GetBookingStatistics()
        {
            var stats = await repo.GetBookingStats();
            return Ok(stats);
        }

    }







}
