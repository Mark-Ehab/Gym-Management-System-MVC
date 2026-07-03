using AutoMapper;
using GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.Presentation.ViewModels.SessionViewModel;

namespace GymManagementSystem.Presentation.MappingProfiles;

public class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<AllSessionsDTO, AllSessionsViewModel>();

        CreateMap<CreateSessionViewModel,CreateSessionDTO>();

        CreateMap<SessionDetailsDTO, SessionDetailsViewModel>();

        CreateMap<EditSessionDTO, EditSessionViewModel>().ReverseMap();

    }
}
