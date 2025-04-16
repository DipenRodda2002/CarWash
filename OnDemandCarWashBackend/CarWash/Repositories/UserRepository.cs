using System;
using CarWash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWash.Models;
using CarWash.Interface;
using Microsoft.AspNetCore.Identity;
using CarWash.Models.DTOs.AddDTOs;
namespace CarWash.Repositories;

public class UserRepository : IUser
{
    private readonly CarWashContext context;
    private readonly UserManager<IdentityUser> _userManager;
    public UserRepository(CarWashContext ct, UserManager<IdentityUser> userManager)
    {
        context = ct;
        _userManager = userManager;
    }

    public async Task<bool> AddUser(User user)
    {
        var res = await context.Users.AnyAsync(x => x.Email == user.Email);
        if (!res)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return true;

        }
        return false;

    }

    public async Task<User> GetUserById(Guid userId)
    {
        var result = await context.Users.Include(x => x.Cars).Include(x => x.Addresses).SingleOrDefaultAsync(x => x.UserId == userId);
        if (result != null)
        {
            return result;
        }
        return null;
    }

    public async Task<IEnumerable<User>> GetUsersByRole(string role)
    {
        var result = await context.Users.Include(x => x.Addresses).Where(x => x.Roles == role).ToListAsync();
        if (result.Any())
        {
            return result;
        }
        return null;
    }

    public async Task<bool> DeleteUser(Guid userId)
    {
        var user = await context.Users
            .Include(u => u.Bookings)
            .Include(u => u.Cars)
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            return false;
        }


        var userBookings = await context.Bookings
            .Where(b => b.CustomerId == userId || b.WasherId == userId)
            .ToListAsync();
        context.Bookings.RemoveRange(userBookings);


        context.Cars.RemoveRange(user.Cars);


        context.Addresses.RemoveRange(user.Addresses);


        context.Users.Remove(user);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var result = await context.Users.Include(x => x.Cars).Include(x => x.Addresses).ToListAsync();
        return result;

    }
    public async Task<User> UpdateUser(Guid userId, User updatedUser)
    {
        var existingUser = await context.Users.SingleOrDefaultAsync(x => x.UserId == userId);
        if (existingUser != null)
        {
            existingUser.Name = updatedUser.Name;
            existingUser.Phone = updatedUser.Phone;
            existingUser.Password = updatedUser.Password;
            existingUser.ProfileImage = updatedUser.ProfileImage;
            existingUser.IsActive = updatedUser.IsActive;
            await context.SaveChangesAsync();
            return updatedUser;

        }
        return null;
    }

    public async Task<User?> GetUserByEmailIdAsync(string emailId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(emailId))
                throw new ArgumentException("Email ID cannot be null or empty", nameof(emailId));

            return await context.Users.FirstOrDefaultAsync(u => u.Email == emailId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error fetching user with email {emailId}", ex);
        }
    }

    public async Task<User> ToggleUser(Guid userId, User user)
    {
        var res = await context.Users.SingleOrDefaultAsync(x=>x.UserId==userId);
        if(res != null){
            res.IsActive = user.IsActive;
            await context.SaveChangesAsync();
            return user;
        }
        return null;

    }

    public async Task<int>TotalCustomers(){
        var result = await context.Users.CountAsync(x=>x.Roles=="Customer");
        Console.WriteLine(result);
        if(result  != 0){
            return result;
        }
        return 0;

    }
    public async Task<int>TotalWashers(){
        var result = await context.Users.CountAsync(x=>x.Roles=="Washer");
        Console.WriteLine(result);
        if(result  != 0){
            return result;
        }
        return 0;

    }

    






}
