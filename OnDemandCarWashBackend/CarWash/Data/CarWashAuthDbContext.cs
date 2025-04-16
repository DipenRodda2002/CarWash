using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Data;

public class CarWashAuthDbContext : IdentityDbContext
{
    public CarWashAuthDbContext(DbContextOptions<CarWashAuthDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var Customer = "e74632e5-0976-4852-9cae-4bc878b4bdfc";
        var Washer = "ed8b11ad-1fd6-4e52-b174-d4daeddf0d83";
        var Admin = "51338190-7684-4dbf-ba1d-d818589af718";
        var roles = new List<IdentityRole>{
            new IdentityRole{
                Id = Customer,
                ConcurrencyStamp = Customer,
                Name = "Customer",
                NormalizedName = "Customer".ToUpper()
            },
            new IdentityRole{
                Id = Washer,
                ConcurrencyStamp = Washer,
                Name = "Washer",
                NormalizedName = "Washer".ToUpper()
            },
            new IdentityRole{
                Id = Admin,
                ConcurrencyStamp = Admin,
                Name = "Admin",
                NormalizedName = "Admin".ToUpper()
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }


}
