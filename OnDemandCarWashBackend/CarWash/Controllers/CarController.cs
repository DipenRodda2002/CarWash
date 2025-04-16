using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarWash.Interface;
using CarWash.Repositories;
using CarWash.Models;
// using CarWash.Models.DTOs.AddDTOs;
using CarWashApp.Models.DTOs;
using CarWash.Models.DTOs.GetDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICar repo;
        private readonly IMapper mapper;
        public CarController(ICar repository, IMapper mappers)
        {
            repo = repository;
            mapper = mappers;
        }
        // 
        // [HttpPost("AddCar")]
        // [Authorize(Roles = "Customer")]
        // public async Task<IActionResult> AddCar([FromBody] CarDTO car)
        // {


        //     if (!ModelState.IsValid)
        //     {

        //         return BadRequest(ModelState);
        //     }

        //     var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     Console.WriteLine(userIdClaim);

        //     if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        //     {

        //         return Unauthorized("Invalid or missing User ID in token");
        //     }
        //     string fileName = null;
        //     if (car.CarImage != null && car.CarImage.Length > 0)
        //     {
        //         var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
        //         Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

        //         fileName = $"{Guid.NewGuid()}_{car.CarImage.FileName}";
        //         var filePath = Path.Combine(uploadsFolder, fileName);

        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             await car.CarImage.CopyToAsync(stream);
        //         }
        //     }
        //     var carDomainModel = new Car
        //     {
        //         Brand = car.Brand,
        //         Model = car.Model,
        //         Year = car.Year,
        //         LicensePlate = car.LicensePlate,
        //         CarImage = fileName,
        //         UserId = Guid.Parse(userIdClaim)
        //     };


        //     var result = await repo.AddCar(carDomainModel);
        //     if (result)
        //     {
        //         return Ok(new { message = "Car added successfully", imagePath = fileName });
        //     }
        //     return BadRequest("NO Car added");
        // }

        [HttpGet("GetUserCars/{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCarsById(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest("Invalid user ID.");
                }
                var result = await repo.GetCarsByUserId(userId);
                if (result.Any())
                {
                    var carDTos = mapper.Map<IEnumerable<GetCarDTO>>(result);
                    return Ok(carDTos);
                }
                return NotFound("No Cars Added");
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
        [HttpPost("AddCar")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddCar([FromForm] CarDTO car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid or missing User ID in token");
            }

            string fileName = null;

            if (car.CarImage != null && car.CarImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
                Directory.CreateDirectory(uploadsFolder); // Create folder if not exists

                fileName = $"{Guid.NewGuid()}_{car.CarImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await car.CarImage.CopyToAsync(stream);
                }
            }

            var carDomainModel = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                LicensePlate = car.LicensePlate,
                CarImage = fileName,
                UserId = userId
            };

            var result = await repo.AddCar(carDomainModel);

            if (result)
            {
                return Ok(new { message = "Car added successfully", imagePath = fileName });
            }

            return BadRequest("No Car added");
        }
        [HttpGet("GetCar-By-Id/{carId}")]
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> GetCarById(int carId)
        {
            try
            {
                if (carId <= 0)
                {
                    return BadRequest("Invalid car ID.");
                }
                var result = await repo.GetCarById(carId);
                if (result != null)
                {
                    var carDTos = mapper.Map<GetCarDTO>(result);
                    return Ok(carDTos);
                }
                return NotFound("No Cars of this Id exists.");
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

        [HttpDelete("RemoveCar-By-Id/{carId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveCar(int carId)
        {
            try
            {
                if (carId <= 0)
                {
                    return BadRequest("Invalid car ID.");
                }
                var result = await repo.DeleteCar(carId);
                if (result)
                {
                    // return Ok("Car Removed");
                    return Ok(new { message = "Car Removed" });
                }
                return NotFound("Enter Valid Car Id");
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
    }
}
