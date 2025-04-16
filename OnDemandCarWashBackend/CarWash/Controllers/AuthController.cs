using System.Text.Json;
using AutoMapper;
using CarWash.Data;
using CarWash.Interface;
using CarWash.Models;
using CarWash.Models.DTOs.AddDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly CarWashContext context;

        public AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService, IMapper mapper, CarWashContext context)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpPost]
        [Route("Register")]
        // [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requestedRoles = registerDTO.Roles.ToList();

            if (requestedRoles.Contains("Admin"))
            {
                var currentUser = HttpContext.User;
                if (currentUser == null || !currentUser.Identity.IsAuthenticated || !currentUser.IsInRole("Admin"))
                {
                    return Unauthorized("Only an Admin can create another Admin account.");
                }
            }

            var identityUser = new IdentityUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerDTO.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    Console.WriteLine($"Identity Error: {error.Description}");
                }
                return BadRequest(identityResult.Errors);
            }
            if (registerDTO.Roles != null && registerDTO.Roles.Any())
            {
                //var normalizedRoles = registerDTO.Roles.Select(r => r.ToUpper()).ToList();

                identityResult = await userManager.AddToRolesAsync(identityUser, registerDTO.Roles);

                if (!identityResult.Succeeded)
                {
                    return BadRequest(identityResult.Errors);
                }
            }
            var user = new User
            {

                UserId = Guid.Parse(identityUser.Id),
                Email = registerDTO.Username,
                Name = registerDTO.Name,
                Phone = registerDTO.Phone,
                Password = registerDTO.Password,
                ProfileImage = registerDTO.ProfileImage,
                CreatedAt = DateTime.UtcNow,
                Roles = requestedRoles.FirstOrDefault()
            };


            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(new { message = "User was registered! Please Login." });
        }

        [HttpPost]
        [Route("Login")]
        // [AllowAnonymous]

        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        {
            var user = await userManager.FindByEmailAsync(loginUser.Username);
            if (user != null)
            {
                var CheckPassword = await userManager.CheckPasswordAsync(user, loginUser.Password);
                if (CheckPassword)
                {


                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenService.GenerateToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);

                    }




                }

            }
            return BadRequest("Invalid Username or Password");
        }
    }
}
