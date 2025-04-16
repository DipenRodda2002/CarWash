using System;
using CarWash.Models;
using CarWash.Data;
using Microsoft.EntityFrameworkCore;
using CarWash.Interface;
namespace CarWash.Repositories;


public class CarRepository : ICar
{
    private readonly CarWashContext context;
    public CarRepository(CarWashContext ct)
    {
        context = ct;
    }

    public async Task<bool> AddCar(Car car)
    {
        var res = await context.Users.AnyAsync(x => x.UserId == car.UserId);
        if (res)
        {
            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();
            return true;

        }
        return false;

    }

    public async Task<IEnumerable<Car>> GetCarsByUserId(Guid userId)
    {
        var result = await context.Cars.Where(x => x.UserId == userId).ToListAsync();
        return result;
    }

    public async Task<Car> GetCarById(int carId){
    var result = await context.Cars.SingleOrDefaultAsync(x=>x.CarId == carId);
    if(result != null){
        return result;
    }
    return null;

}
public async Task<bool>DeleteCar(int carId){
    var result = await context.Cars.SingleOrDefaultAsync(x=>x.CarId == carId);
    if(result != null){
        context.Cars.Remove(result);
        await context.SaveChangesAsync();
        return true;
    }
    return false;

}




}
