using System;
using AutoMapper;
using CarWash.Models;
using CarWash.Models.DTOs;
using CarWash.Models.DTOs.AddDTOs;
using CarWash.Models.DTOs.GetDTO;
using CarWashApp.Models.DTOs;

namespace CarWash.Mapping;

public class AutoMaperProfile : Profile
{
    public AutoMaperProfile()
    {
        CreateMap<Address, AddressDTO>().ReverseMap();
        CreateMap<User, WasherDTO>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User,GetUserDTO>().ReverseMap();
        CreateMap<Car,GetCarDTO>().ReverseMap();
        CreateMap<Address,GetAddressDTOs>().ReverseMap();
        CreateMap<User,UserResponseDto>().ReverseMap();
        CreateMap<User,LoginUserDto>().ReverseMap();
        CreateMap<User, UserRegisterDTO>().ReverseMap();
        CreateMap<Bookings,BookingRequest>().ReverseMap();
        CreateMap<ReviewRating,GiveReviewDTO>().ReverseMap();
        CreateMap<ReviewRating,GetReviewsDTO>().ReverseMap();
        CreateMap<User,UpdateUserDTO>().ReverseMap();
        CreateMap<WashPackage,GetPackagesDTO>().ReverseMap();
        CreateMap<Bookings,GetWasherDTOs>().ReverseMap();
        CreateMap<Bookings,GetAvailableWasherDTO>().ReverseMap();
        CreateMap<Bookings,UpdateBookingStatusDTO>().ReverseMap();
        CreateMap<Bookings,GetRecentBookingDTO>()
        .ForMember(dest=>dest.CustomerName,opt=>opt.MapFrom(src=>src.Customer.Name))
        .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Car.Model))
        .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.WashPackage.PackageName));

        CreateMap<User,ToggleDTO>().ReverseMap();
        CreateMap<Bookings,GetAllBookingsDTO>()
        .ForMember(dest=>dest.CustomerName,opt=>opt.MapFrom(src=>src.Customer.Name))
        .ForMember(dest => dest.WasherName, opt => opt.MapFrom(src => src.Washer.Name))
        .ForMember(dest => dest.AddressName, opt => opt.MapFrom(src => src.Address.City))
        .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.WashPackage.PackageName))
        .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.Car.Model));
        CreateMap<Bookings, BookingDetailsDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.WasherName, opt => opt.MapFrom(src => src.Washer.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment.PaymentMethod))
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Payment.PaymentId))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Payment.PaymentAmount));
    }
    
    

}
