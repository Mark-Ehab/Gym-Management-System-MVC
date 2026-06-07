using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.MappingProfiles;

public sealed class TrainerMappingProfile : Profile
{
    public TrainerMappingProfile()
    {
        CreateMap<Trainer, AllTrainersDTO>();
      
        CreateMap<Trainer, TrainerDetailsDTO>()
            .ForMember(dest => dest.BuildingNumber, opts => opts.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.Street, opts => opts.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City));
      
        CreateMap<TrainerCreateDTO,Trainer>()
            .ForMember(dest=>dest.Address,opts=>opts.MapFrom(src=>new Address()
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City
            }));

        CreateMap<Trainer, TrainerToBeEditedDTO>()
            .ForMember(dest => dest.BuildingNumber, opts => opts.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.Street, opts => opts.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City));
            
        CreateMap<TrainerToBeEditedDTO,Trainer>()
            .ForMember(dest => dest.Address, opts => opts.MapFrom(src => new Address()
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City,
            }))
            .ForMember(dest => dest.Name, opts => opts.Ignore());
    }
}
