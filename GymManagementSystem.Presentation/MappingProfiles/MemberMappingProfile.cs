using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;
using GymManagementSystem.Presentation.ViewModels.MemberViewModels;

namespace GymManagementSystem.Presentation.MappingProfiles;

public sealed class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        CreateMap<AllMembersDTO, AllMembersViewModel>();

        CreateMap<MemberDetailsDTO, MemberDetailsViewModel>()
            .ForMember(dest => dest.Gender, opts => opts.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.DateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            .ForMember(dest => dest.MembershipStartDate, opts => opts.MapFrom(src => src.MembershipStartDate.ToString()))
            .ForMember(dest => dest.MembershipEndDate, opts => opts.MapFrom(src => src.MembershipEndDate.ToString()));

        CreateMap<HealthRecordDTO, HealthRecordViewModel>()
            .AfterMap((src, dest) =>
            {
                dest.BloodType = src.BloodType switch
                {
                    BloodType.APositive => "A+",
                    BloodType.BPositive => "B+",
                    BloodType.OPositive => "O+",
                    BloodType.ABPositive => "AB+",
                    BloodType.ANegative => "A-",
                    BloodType.BNegative => "B-",
                    BloodType.ONegative => "O-",
                    _ => "AB-",
                };
            });

        CreateMap<HealthRecordViewModel, HealthRecordDTO>()
             .ForMember(dest => dest.BloodType,
                 opts => opts.MapFrom(src => ToBloodTypeEnum(src.BloodType)));

        CreateMap<MemberCreateViewModel, MemberCreateDTO>()
            .AfterMap((src,dest)=>
                dest.HealthRecord.Note = src.HealthRecord.Note is null?"No Notes.": src.HealthRecord.Note);

        CreateMap<MemberToBeEditedDTO, MemberToBeEditedViewModel>();

        CreateMap<MemberToBeEditedViewModel, MemberToBeEditedDTO>()
            .ForMember(dest => dest.Photo, opts => opts.Ignore())
            .ForMember(dest => dest.Name, opts => opts.Ignore());
     }

    private static BloodType ToBloodTypeEnum(string bloodType)
    {
        return bloodType.Trim().ToUpperInvariant() switch
        {
            "A+" => BloodType.APositive,
            "B+" => BloodType.BPositive,
            "O+" => BloodType.OPositive,
            "AB+" => BloodType.ABPositive,
            "A-" => BloodType.ANegative,
            "B-" => BloodType.BNegative,
            "O-" => BloodType.ONegative,
            "AB-" => BloodType.ABNegative,
        };
    }
}
