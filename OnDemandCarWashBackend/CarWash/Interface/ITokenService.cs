using System;
using CarWash.Models;
using Microsoft.AspNetCore.Identity;

namespace CarWash.Interface;

public interface ITokenService
{
    string GenerateToken(IdentityUser
     user,List<string>roles);


}
