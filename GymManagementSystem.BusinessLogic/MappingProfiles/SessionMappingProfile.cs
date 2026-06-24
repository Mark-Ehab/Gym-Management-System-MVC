using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.CategoryDTOs;
using GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.MappingProfiles;

public sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<Session, AllSessionsDTO>()
            .ForMember(dest => dest.TrainerName, opts => opts.MapFrom(src => src.Trainer.Name))
            .ForMember(dest => dest.CategoryName ,opts => opts.MapFrom(src => src.Category.Name));

        CreateMap<Category, CategorySelectDTO>();

        CreateMap<Trainer, TrainerSelectDTO>();

        CreateMap<CreateSessionDTO, Session>();

        CreateMap<Session, SessionDetailsDTO>()
            .ForMember(dest => dest.TrainerName, opts => opts.MapFrom(src => src.Trainer.Name))
            .ForMember(dest => dest.CategoryName, opts => opts.MapFrom(src => src.Category.Name));

        CreateMap<Session, EditSessionDTO>().ReverseMap();
    }
}
