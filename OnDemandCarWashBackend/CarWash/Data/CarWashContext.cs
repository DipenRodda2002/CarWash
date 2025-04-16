using System;
using Microsoft.EntityFrameworkCore;
using CarWash.Models;

namespace CarWash.Data;

public class CarWashContext : DbContext
{

    public CarWashContext(DbContextOptions<CarWashContext> options)
        : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<WashPackage> WashPackages { get; set; }
    public DbSet<AddOn> AddOns { get; set; }
    public DbSet<Bookings> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    // public DbSet<Review> Reviews { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<ReviewRating> Reviews { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bookings>()
                    .HasOne(b => b.Customer)
                    .WithMany(u => u.Bookings) // Ensure User has ICollection<Bookings>
                    .HasForeignKey(b => b.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // Washer Mapping
        modelBuilder.Entity<Bookings>()
            .HasOne(b => b.Washer)
            .WithMany() // No ICollection<Bookings> in User for Washers
            .HasForeignKey(b => b.WasherId)
            .OnDelete(DeleteBehavior.Restrict);

        // Car Mapping
        modelBuilder.Entity<Bookings>()
            .HasOne(b => b.Car)
            .WithMany(c=>c.Bookings) // No ICollection<Bookings> in Car
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        // Address Mapping
        modelBuilder.Entity<Bookings>()
            .HasOne(b => b.Address)
            .WithMany()
            .HasForeignKey(b => b.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        // Package Mapping
        modelBuilder.Entity<Bookings>()
            .HasOne(b => b.WashPackage)
            .WithMany(c=>c.Bookings)
            .HasForeignKey(b => b.PackageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Payment Mapping
        modelBuilder.Entity<Bookings>()
            .HasOne(b => b.Payment)
            .WithOne(p => p.Bookings) // Assuming one-to-one relationship
            .HasForeignKey<Payment>(p => p.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<ReviewRating>()
    .HasOne(r => r.Booking)
    .WithMany()
    .HasForeignKey(r => r.BookingId)
    .OnDelete(DeleteBehavior.NoAction);  // Prevents cascading delete

        modelBuilder.Entity<ReviewRating>()
            .HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);  // Prevents cascading delete

        modelBuilder.Entity<ReviewRating>()
            .HasOne(r => r.Washer)
            .WithMany()
            .HasForeignKey(r => r.WasherId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Address>()
    .HasOne(a => a.User)
    .WithMany(u => u.Addresses)
    .HasForeignKey(a => a.UserId)
    .OnDelete(DeleteBehavior.Cascade); // Deletes all addresses when user is deleted

    // modelBuilder.Entity<Car>()
    //     .HasOne(c => c.User)
    //     .WithMany(u => u.Cars)  // Ensure User has a Cars collection
    //     .HasForeignKey(c => c.UserId)
    //     .OnDelete(DeleteBehavior.Cascade);  // Enable Cascade Delete

        base.OnModelCreating(modelBuilder);
    }
    //     modelBuilder.Entity<Bookings>()
    //         .HasOne(b => b.Customer)
    //         .WithMany(u => u.Bookings)
    //         .HasForeignKey(b => b.CustomerId)
    //         .OnDelete(DeleteBehavior.Restrict);

    //     modelBuilder.Entity<Bookings>()
    //         .HasOne(b => b.Washer)
    //         .WithMany() // Washer may not have a dedicated Bookings collection
    //         .HasForeignKey(b => b.WasherId)
    //         .OnDelete(DeleteBehavior.Restrict);

    //     base.OnModelCreating(modelBuilder);

    //     modelBuilder.Entity<Bookings>()
    //         .HasOne(b => b.Customer)
    //         .WithMany()
    //         .HasForeignKey(b => b.CustomerId)
    //         .OnDelete(DeleteBehavior.NoAction); // Prevents cascade path issue

    //     modelBuilder.Entity<Bookings>()
    //         .HasOne(b => b.Washer)
    //         .WithMany()
    //         .HasForeignKey(b => b.WasherId)
    //         .OnDelete(DeleteBehavior.NoAction); // Prevents cascade path issue

    //     modelBuilder.Entity<Bookings>()
    //         .HasOne(b => b.Car)
    //         .WithMany()
    //         .HasForeignKey(b => b.CarId)
    //         .OnDelete(DeleteBehavior.NoAction); // This is causing the issue
    // }
    // public DbSet<Washer> Washers 3{ get; set; }
    //public DbSet<PromoCode> PromoCodes { get; set; }
    //public DbSet<Leaderboard> Leaderboards { get; set; }
    // public DbSet<OrderAddOn> OrderAddOns { get; set; }

    // public DbSet<Admin> Admins {get;set;}

    //     protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);

    //     // Orders -> Users
    //     modelBuilder.Entity<Bookings>()
    //     .HasOne(o => o.Customer)
    //     .WithMany(u => u.Bookings)
    //     .HasForeignKey(o => o.CustomerId)
    //     .OnDelete(DeleteBehavior.Restrict);
    //   // Prevent cascade delete
    //     modelBuilder.Entity<Bookings>()
    //     .HasOne(o => o.Washer)
    //     .WithMany()
    //     .HasForeignKey(o => o.WasherId)
    //     .OnDelete(DeleteBehavior.Restrict);

    //     // Orders -> Cars
    //     modelBuilder.Entity<Bookings>()
    //         .HasOne(o => o.Car)
    //         .WithMany()
    //         .HasForeignKey(o => o.CarId)
    //         .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

    //     // Orders -> WashPackages
    //     modelBuilder.Entity<Bookings>()
    //         .HasOne(o => o.WashPackage)
    //         .WithMany()
    //         .HasForeignKey(o => o.PackageId)
    //         .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

    //      modelBuilder.Entity<Bookings>()
    //         .HasOne(o => o.Address) // Assuming Address is a navigation property in your Order model
    //         .WithMany()
    //         .HasForeignKey(o => o.AddressId)
    //         .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

    // //    modelBuilder.Entity<AddOn>()
    // //         .Property(a => a.Price)
    // //         .HasPrecision(18, 2);  // 18 digits in total, 2 decimal places


    //     // For Order TotalPrice
    //     modelBuilder.Entity<Bookings>()
    //         .Property(o => o.TotalPrice)
    //         .HasPrecision(18, 2);

    //     // For Payment PaymentAmount
    //     modelBuilder.Entity<Payment>()
    //         .Property(p => p.PaymentAmount)
    //         .HasPrecision(18, 2);

    //     // For WashPackage Price
    //     modelBuilder.Entity<WashPackage>()
    //         .Property(w => w.Price)
    //         .HasPrecision(18, 2);
    // }




}


