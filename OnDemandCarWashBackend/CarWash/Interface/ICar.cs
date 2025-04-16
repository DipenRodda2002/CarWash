using System;
using CarWash.Models;

namespace CarWash.Interface;

public interface ICar
{

    Task<bool>AddCar(Car car);

    Task<IEnumerable<Car>> GetCarsByUserId(Guid userId);
    Task<Car> GetCarById(int carId);
    Task<bool>DeleteCar(int carId);

}
