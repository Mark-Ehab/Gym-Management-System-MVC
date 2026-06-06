using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.MappingProfiles;

public sealed class PlanMappingProfile : Profile
{
    public PlanMappingProfile()
    {
        CreateMap<Plan, AllPlansDTO>()
            .ForMember(dest=>dest.Status,opts=>opts.MapFrom(src=>src.IsActive));
        
        CreateMap<Plan, PlanDetailsDTO>()
            .ForMember(dest=>dest.Status,opts=>opts.MapFrom(src=>src.IsActive));

        CreateMap<Plan, PlanToBeEditedDTO>();

        CreateMap<PlanToBeEditedDTO, Plan>()
            .ForMember(dest=>dest.Name,opts=>opts.Ignore());
    }
}
