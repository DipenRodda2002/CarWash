using System;
using CarWash.Models;
namespace CarWash.Interface;

public interface IAddress
{
    Task<bool> AddAddress(Address address);
    Task<Address> UpdateAddress(int AddressId, Address updateAddress);
    Task<IEnumerable<Address>> GetAddressByUserId(Guid userId);
     Task<bool>DeleteAddress(int AddressId);

}
