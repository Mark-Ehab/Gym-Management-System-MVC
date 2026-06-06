using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.MappingProfiles;

public sealed class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        CreateMap<Member, AllMembersDTO>();

        CreateMap<Member, MemberDetailsDTO>()
            .ForMember(dest => dest.Address, opts => opts.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            .AfterMap((src, dest) =>
            {
                dest.PlanName = src.Memberships.FirstOrDefault()?.Plan.Name;
                dest.MembershipStartDate = src.Memberships.FirstOrDefault()?.StartDate;
                dest.MembershipStartDate = src.Memberships.FirstOrDefault()?.EndDate;
            });

        CreateMap<HealthRecord, HealthRecordDTO>().ReverseMap();

        CreateMap<MemberCreateDTO, Member>()
            .ForMember(dest => dest.Address, opts => opts.MapFrom(src => new Address()
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City,
            }));

        CreateMap<Member, MemberToBeEditedDTO>()
            .ForMember(dest => dest.BuildingNumber, opts => opts.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.Street, opts => opts.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City));

        CreateMap<MemberToBeEditedDTO, Member>()
            .ForMember(dest => dest.Address, opts => opts.MapFrom(src => new Address()
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City
            }))
            .ForMember(dest => dest.Name, opts => opts.Ignore())
            .ForMember(dest => dest.Photo, opts => opts.Ignore());
    }
}
