using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.AnalyticsDTOs;
using GymManagementSystem.Presentation.ViewModels.AnalyticsViewModels;

namespace GymManagementSystem.Presentation.MappingProfiles;

public sealed class AnalyticsMappingProfile : Profile
{
    public AnalyticsMappingProfile()
    {
        CreateMap<HomeAnalyticsDTO, HomeAnalyticsViewModel>();
    }
}
