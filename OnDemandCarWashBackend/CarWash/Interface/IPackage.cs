using System;
using System;
using CarWash.Models;
using CarWash.Interface;
using CarWash.Models.DTOs;
using CarWash.Models.DTOs.AddDTOs;

namespace CarWash.Interface;

public interface IPackage
{
    Task<bool> AddPackageAsync(WashPackage package);
    Task<bool> UpdatePackageAsync(int packageId, WashPackage package);
    // Task<WashPackage> GetPackageById(int packageId);
    Task<bool> DeletePackage(int packageId);
    Task<WashPackage> GetPackageByName(string packageName);
    Task<IEnumerable<WashPackage>>GetAllPackages();


}
