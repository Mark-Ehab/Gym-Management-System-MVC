using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.Presentation.ViewModels.MembershipViewModels;

namespace GymManagementSystem.Presentation.MappingProfiles;

public sealed class MembershipMappingProfile : Profile
{
    public MembershipMappingProfile()
    {
        CreateMap<AllMembershipsDTO,AllMembershipsViewModel>();

        CreateMap<CreateMembershipViewModel,CreateMembershipDTO>()
            .ForMember(dest => dest.MemberId, opts => opts.MapFrom(src => src.Member))
            .ForMember(dest => dest.PlanId, opts => opts.MapFrom(src => src.Plan));
    }
}
