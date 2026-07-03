using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.Presentation.ViewModels.TrainerViewModels;

namespace GymManagementSystem.Presentation.MappingProfiles;

public sealed class TrainerMappingProfile : Profile
{
    public TrainerMappingProfile()
    {
        CreateMap<AllTrainersDTO,AllTrainersViewModel>()
            .ForMember(dest=>dest.Specialization,opts=>opts.MapFrom(src => src.Speciality.ToString()));

        CreateMap<TrainerDetailsDTO, TrainerDetailsViewModel>()
            .ForMember(dest => dest.DateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            .ForMember(dest => dest.Specialization, opts => opts.MapFrom(src => src.Speciality.ToString()))
            .ForMember(dest => dest.Address, opts => opts.MapFrom(src => $"{src.BuildingNumber} - {src.Street} - {src.City}"));

        CreateMap<TrainerCreateViewModel, TrainerCreateDTO>()
            .ForMember(dest => dest.Speciality, opts => opts.MapFrom(src => src.Specialties));
        
        CreateMap<TrainerToBeEditedDTO, TrainerToBeEditedViewModel>()
            .ForMember(dest => dest.Specialties, opts => opts.MapFrom(src => src.Speciality));
       
        CreateMap<TrainerToBeEditedViewModel,TrainerToBeEditedDTO>()
            .ForMember(dest => dest.Speciality, opts => opts.MapFrom(src => src.Specialties))
            .ForMember(dest => dest.Name, opts => opts.Ignore());
    }
}
