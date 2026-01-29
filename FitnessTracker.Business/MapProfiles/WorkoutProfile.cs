using AutoMapper;
using FitnessTracker.Business.DTOs;
using FitnessTracker.Core.Entities;

namespace FitnessTracker.Business.MapProfiles;

public class WorkoutProfile : Profile
{
    public WorkoutProfile ()
    {
        CreateMap<SetEntity, SetDto>();
        CreateMap<ExerciseEntity, ExerciseDto>();
        CreateMap<WorkoutEntity, WorkoutDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        CreateMap<PhotoEntity, string>()
            .ForMember(dest => dest, opt => opt.MapFrom(src => src.FilePath.ToString()));
    }
}
