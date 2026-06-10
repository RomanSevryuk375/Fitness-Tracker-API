using AutoMapper;
using FitnessTracker.Business.DTOs;
using FitnessTracker.Core.AggregateRoots.Workouts;

namespace FitnessTracker.Business.MapProfiles;

public class WorkoutProfile : Profile
{
    public WorkoutProfile ()
    {
        CreateMap<Set, SetDto>();
        CreateMap<Exercise, ExerciseDto>();
        CreateMap<Workout, WorkoutDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.ProgressPhotos, opt =>
                    opt.MapFrom(src => src.Photos.Select(p => p.FilePath).ToList()));
    }
}
