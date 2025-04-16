using System;
using CarWash.Data;
using CarWash.Interface;
using CarWash.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repositories;

public class AddressRepository:IAddress
{
    private readonly CarWashContext context;
    public AddressRepository(CarWashContext ct)
    {
        context = ct;
    }

    public async Task<bool> AddAddress(Address address)
    {
        var res = await context.Users.AnyAsync(x => x.UserId == address.UserId);
        if (res)
        {
            var result = await context.Addresses.AddAsync(address);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    public async Task<Address> UpdateAddress(int AddressId, Address updateAddress)
    {
        var existingAddress = await context.Addresses.SingleOrDefaultAsync(x => x.AddressId == AddressId);
        if (existingAddress != null)
        {
            existingAddress.Street = updateAddress.Street;
            existingAddress.City = updateAddress.City;
            existingAddress.State = updateAddress.State;
            existingAddress.Pincode = updateAddress.Pincode;
            existingAddress.AddressType=updateAddress.AddressType;
            await context.SaveChangesAsync();
            return updateAddress;

        }
        return null;
    }
    public async Task<IEnumerable<Address>> GetAddressByUserId(Guid userId){
        var result = await context.Addresses.Where(x=>x.UserId==userId).ToListAsync();
        return result;

    }
    public async Task<bool>DeleteAddress(int addressId){
        var res = await context.Addresses.SingleOrDefaultAsync(x=>x.AddressId==addressId);
        if(res != null){
            context.Addresses.Remove(res);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    
}
