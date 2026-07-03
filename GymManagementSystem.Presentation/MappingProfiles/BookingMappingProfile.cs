using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.BookingDTOs;
using GymManagementSystem.Presentation.ViewModels.BookingViewModels;

namespace GymManagementSystem.Presentation.MappingProfiles;

public sealed class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<SessionMemberBookingDTO, SessionMemberBookingViewModel>();
    }
}
