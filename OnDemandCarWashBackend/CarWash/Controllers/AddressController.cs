using CarWash.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarWash.Models.DTOs;
using CarWash.Models;
using AutoMapper;
using CarWash.Models.DTOs.GetDTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddress repo;
        private readonly IMapper mapper;
        public AddressController(IAddress repository, IMapper mappers)
        {
            repo = repository;
            mapper = mappers;
        }
        [HttpPost("AddUserAddress")]
        [Authorize(Roles = "Customer,Washer")]
        public async Task<IActionResult> AddAdress([FromBody] AddressDTO address)
        {
            try
            {
                if (address == null)
                {
                    return BadRequest("Address data is required.");
                }
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var DomainModel = new Address
                {
                    Street = address.Street,
                    City = address.City,
                    Pincode = address.Pincode,
                    State = address.State,
                    AddressType = address.AddressType,
                    UserId = Guid.Parse(userIdClaim)
                };
                var result = await repo.AddAddress(DomainModel);
                if (result)
                {
                    return Ok( new {message="Address Added Successfully"});
                    
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPut("UpdateAddress/{addressID}")]
        [Authorize(Roles = "Customer,Washer")]
        public async Task<IActionResult> UpdateAddress(int addressID, AddressDTO address)
        {
            try
            {
                if (address == null)
                {
                    return BadRequest("Address data is required.");
                }

                if (addressID <= 0)
                {
                    return BadRequest("Invalid address ID.");
                }
                var AddressDomainModel = new Address
                {
                    Street = address.Street,
                    State = address.State,
                    City = address.City,
                    Pincode = address.Pincode,
                    AddressType = address.AddressType
                };
                var result = await repo.UpdateAddress(addressID, AddressDomainModel);
                if (result != null)
                {
                    return Ok(new {message="Address Updated"});
                }
                return NotFound("Address Not Found");
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
        [HttpGet("GetAllAddress-userId")]
        [Authorize(Roles = "Customer,Washer")]
        public async Task<IActionResult> GetAllAddress(Guid UserId)
        {
            try
            {
                if (UserId == Guid.Empty)
                {
                    return BadRequest("Invalid user ID.");
                }


                var result = await repo.GetAddressByUserId(UserId);
                if (result.Any())
                {
                    var addressDTOs = mapper.Map<IEnumerable<GetAddressDTOs>>(result);
                    return Ok(addressDTOs);
                    
                }
                return NotFound("No Addresses Added.");
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
        [HttpDelete("DeleteAddress-by-ID/{addressId}")]
        [Authorize(Roles = "Customer,Washer")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            try
            {
                if (addressId <= 0)
                {
                    return BadRequest("Invalid address ID.");
                }

                var result = await repo.DeleteAddress(addressId);
                if (result)
                {
                    return Ok(new {message="Address Deleted Successfully"});
                }
                return BadRequest();
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
