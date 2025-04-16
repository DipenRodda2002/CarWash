using System;

using CarWash.Data;
using CarWash.Interface;
using CarWash.Models;
using CarWash.Models.DTOs;
using Microsoft.EntityFrameworkCore;
namespace CarWash.Repositories;

public class PackageRepository:IPackage
{
    private readonly CarWashContext context;
    public PackageRepository(CarWashContext ct){
        context = ct;
    }

    public async Task<bool> AddPackageAsync(WashPackage package){
        var res = await context.WashPackages.AnyAsync(x=>x.PackageId==package.PackageId);
        if(!res){
            await context.WashPackages.AddAsync(package);
            await context.SaveChangesAsync();
            return true;
                
        }
        return false;
    }
    public async Task<bool> UpdatePackageAsync(int packageId, WashPackage package){
        var existingPackage = await context.WashPackages.FirstOrDefaultAsync(x=>x.PackageId==packageId);
        if(existingPackage!=null){
            existingPackage.Price=package.Price;
            existingPackage.Description=package.Description;
            existingPackage.Duration=package.Duration;
            existingPackage.PackageName=package.PackageName;
            await context.SaveChangesAsync();
            return true;

            
        }
        return false;
    }
    public async Task<bool> DeletePackage(int packageId){
        try{
            if(await context.WashPackages.AnyAsync(p=>p.PackageId==packageId)){
                var package = await context.WashPackages.FirstOrDefaultAsync(p=>p.PackageId==packageId);
                if(package==null){
                    return false;
                }
                context.WashPackages.Remove(package);
                await context.SaveChangesAsync();
                return true;
            }
            return true;
        }
        catch(Exception ex){
            Console.WriteLine("Error : "+ex);
            return false;
        }
    }
    public async Task<WashPackage> GetPackageByName(string packageName){
        var result = await context.WashPackages.SingleOrDefaultAsync(x=>x.PackageName==packageName);
        if(result != null){
            return result;
        }
        return null;
    }

    public async Task<IEnumerable<WashPackage>>GetAllPackages(){
        var result = await context.WashPackages.ToListAsync();
        if(result.Any()){
            return result;
        }
        return new List<WashPackage>();
    }
    

}
