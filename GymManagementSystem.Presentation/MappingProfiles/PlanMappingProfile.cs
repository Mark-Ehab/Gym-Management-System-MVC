using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.Presentation.ViewModels.PlanViewModels;

namespace GymManagementSystem.Presentation.MappingProfiles;

public sealed class PlanMappingProfile : Profile
{
    public PlanMappingProfile()
    {
        CreateMap<AllPlansDTO, AllPlansViewModel>()
            .ForMember(dest=>dest.Active,opts=>opts.MapFrom(src=>src.Status));

        CreateMap<PlanDetailsDTO,PlanDetailsViewModel>()
            .ForMember(dest=>dest.Status,opts=>opts.MapFrom(src => SetToActive(src.Status)));

        CreateMap<PlanToBeEditedDTO, PlanToBeEditedViewModel>().ReverseMap();
    }

    private string SetToActive(bool status)
        => status ? "Active" : "Inactive";
}
