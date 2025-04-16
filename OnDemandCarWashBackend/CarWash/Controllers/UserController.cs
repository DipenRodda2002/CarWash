using AutoMapper;
using CarWash.Interface;
using CarWash.Models;
using CarWash.Models.DTOs.AddDTOs;
using CarWash.Models.DTOs.GetDTO;
using CarWash.Repositories;
using CarWashApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles="Customer,Washer,Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUser repo;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        public UserController(IUser repository, IMapper mappers, ITokenService tokenServices)
        {
            repo = repository;
            mapper = mappers;
            tokenService = tokenServices;
        }

        [HttpPost("AddUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] UserDTO user)
        {
            var userDomainModel = new User
            {

                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                ProfileImage = user.ProfileImage,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                Roles = user.Role

            };
            var result = await repo.AddUser(userDomainModel);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("GetAllUsers")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {

            var result = await repo.GetAllUsers();

            if (result.Any())
            {
                var userDTOs = mapper.Map<IEnumerable<GetUserDTO>>(result);

                return Ok(userDTOs);
            }
            return NotFound("No Users Found");
        }
        [HttpGet("GetUserById")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
            var result = await repo.GetUserById(Id);
            if (result != null)
            {

                var userDTOs = mapper.Map<GetUserDTO>(result);
                return Ok(userDTOs);
            }
            return NotFound("User Not Found");
        }
        [HttpGet("UserGetByRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {

            var result = await repo.GetUsersByRole(role);
            if (result.Any())
            {
                var userDTOs = mapper.Map<IEnumerable<GetUserDTO>>(result);
                return Ok(userDTOs);


            }
            return NotFound("User Not Found");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var isDeleted = await repo.DeleteUser(userId);

            if (!isDeleted)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User deleted successfully" });
        }

        [HttpGet("GetUserByEmailId/{emailId}")]
        [Authorize(Roles = "Admin,Customer,Washer")]
        public async Task<IActionResult> GetUserByEmailId([FromRoute] string emailId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailId))
                    return BadRequest("Email ID cannot be empty");

                var userDomainModel = await repo.GetUserByEmailIdAsync(emailId);

                if (userDomainModel == null)
                    return NotFound(new { message = "User not found" });

                var userDtos = mapper.Map<UserResponseDto>(userDomainModel);
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching user details", error = ex.Message });
            }
        }

        [HttpPatch("UpdateUser/{userID}")]
        public async Task<IActionResult> UpdateUser(Guid userID, UpdateUserDTO user)
        {
            var res = mapper.Map<User>(user);
            var result = await repo.UpdateUser(userID, res);
            if (result != null)
            {
                return Ok(new { message = "User Updated" });
            }
            return NotFound("User Not Found");
        }
        [HttpPut("UpdateStatus/{userId}")]
        public async Task<IActionResult> ToggleStatus(Guid userId, ToggleDTO userdto)
        {
            var res = mapper.Map<User>(userdto);
            var result = await repo.ToggleUser(userId, res);
            if (result != null)
            {
                return Ok(new { message = "Updated Toggle" });
            }
            return NotFound("User Not Found");
        }

        [HttpGet("TotalCustomers")]
        public async Task<IActionResult>TotalCustomers(){
            var res = await repo.TotalCustomers();
            if(res !=  0){
                return  Ok(new {totalCustomers=res});
            }
            return NotFound(new {totalCustomers=res});
        }
        [HttpGet("TotalWashers")]
        public async Task<IActionResult>TotalWashers(){
            var res = await repo.TotalWashers();
            if(res !=  0){
                return  Ok(new {totalWashers=res});
            }
            return NotFound(new {totalWashers=res});
        }
    }

}
