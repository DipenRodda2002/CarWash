using CarWash.Interface;
using CarWash.Models;
using CarWash.Models.DTOs.AddDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment repo;
        public PaymentController(IPayment repository)
        {
            repo = repository;
        }
        // 

    // [Authorize(Roles="Customer")]
        
    [HttpPost("ProcessPayment")]
    
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentRequest)
    {
        if (paymentRequest == null)
            return BadRequest("Invalid payment data.");

        bool isSuccess = await repo.ProcessPaymentAsync(
            paymentRequest.BookingId, 
            paymentRequest.PaymentAmount, 
            paymentRequest.PaymentMethod
        );
       // Console.WriteLine($"Received Payment: BookingId = {paymentRequest.BookingId}, Amount = {paymentRequest.PaymentAmount}, Method = {paymentRequest.PaymentMethod}");

        if (isSuccess)
            return Ok(new {bookingId=paymentRequest.BookingId,message="Payment successful!"});
        else
            return BadRequest(new {message="Payment failed! Please check the booking details."});
    }
    }
}
