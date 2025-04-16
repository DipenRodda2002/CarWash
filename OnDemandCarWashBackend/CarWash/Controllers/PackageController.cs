using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarWash.Interface;
using CarWash.Models;
using CarWash.Models.DTOs;
using CarWash.Models.DTOs.AddDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CarWash.Models.DTOs.GetDTO;

namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackage repo;
        private readonly IMapper mapper;
        public PackageController(IPackage repository,IMapper _mapper)
        {
            repo = repository;
            mapper = _mapper;
        }

        [HttpPost("AddPackage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPackage([FromBody] WashPackageDTO washPackage)
        {
            try
            {
                if (washPackage == null)
                {
                    return BadRequest("Package details are required.");
                }
                var PackageDomainModel = new WashPackage
                {
                    PackageName = washPackage.PackageName,
                    Description = washPackage.Description,
                    Price = washPackage.Price,
                    Duration = washPackage.Duration


                };
                var result = await repo.AddPackageAsync(PackageDomainModel);
                if (result)
                {
                    return Ok(new {message="Package Added"});
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

        [HttpPut("UpdatePackage/{packageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePackage([FromRoute]int packageId, [FromBody] WashPackageDTO package)
        {
            
            var PackageDomainModel = new WashPackage
            {
                PackageName = package.PackageName,
                Description = package.Description,
                Price = package.Price,
                Duration = package.Duration


            };
            await repo.UpdatePackageAsync(packageId, PackageDomainModel);
            return Ok(new {message="Package Updated"});
        }

        [HttpGet("Get-package-byName/{packageName}")]
        public async Task<IActionResult> GetPackageById(string packageName){
            try{
                
                var result= await repo.GetPackageByName(packageName);
                if(result != null){
                    return Ok(result);
                }
                return BadRequest("No Packages Found");
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
        [HttpGet("GetAllPackages")]
        public async Task<IActionResult> GetAllPackages(){
            var result = await repo.GetAllPackages();
            
            if(result.Any()){
                var res = mapper.Map<IEnumerable<GetPackagesDTO>>(result);
                return Ok(res);
            }
            return BadRequest("No Packages Added");
        }

        [HttpDelete("Deletepackage/{packageId}")]
        public async Task<IActionResult> DeletePackage(int packageId){
            var result = await repo.DeletePackage(packageId);
            if(result){
                return Ok(new {message="Package Deleted Succesfully"});
            }
            return BadRequest("No Packages Found ");
        }
       


    }
}
