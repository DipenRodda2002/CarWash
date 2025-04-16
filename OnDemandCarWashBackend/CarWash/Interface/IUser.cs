using System;
using CarWash.Models;
using CarWashApp.Models.DTOs;

namespace CarWash.Interface;

public interface IUser
{
    Task<bool> AddUser(User user);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(Guid userId);
    Task<IEnumerable<User>> GetUsersByRole(string role);
    Task<User> ToggleUser(Guid userId, User user);
    Task<User> UpdateUser(Guid userId, User updatedUser);

    Task<bool> DeleteUser(Guid userId);
    Task<User?> GetUserByEmailIdAsync(string emailId);
    Task<int>TotalCustomers();
    Task<int>TotalWashers();

}
