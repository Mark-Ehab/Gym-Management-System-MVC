using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.BookingDTOs;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.MappingProfiles;

public sealed class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, SessionMemberBookingDTO>()
            .ForMember(dest=>dest.MemberName,opts => opts.MapFrom(src => src.Member.Name))
            .ForMember(dest => dest.BookingId, opts => opts.MapFrom(src => src.Id));
    }
}
