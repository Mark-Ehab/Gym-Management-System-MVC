using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.MappingProfiles;

public sealed class MembershipMappingProfile : Profile
{
    public MembershipMappingProfile()
    {
        CreateMap<Membership, AllMembershipsDTO>()
            .ForMember(dest => dest.MemberName, opts => opts.MapFrom(src => src.Member.Name))
            .ForMember(dest => dest.PlanName, opts => opts.MapFrom(src => src.Plan.Name))
            .ForMember(dest => dest.EndDate, opts => opts.Ignore());
    }
}
