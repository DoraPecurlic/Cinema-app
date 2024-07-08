using AutoMapper;
using Cinema.Model;
using DTO.ProjectionModel;

namespace Cinema.Mapper
{
    public class ProjectionProfile : Profile
    {
        public ProjectionProfile()
        {
            CreateMap<Projection, GetProjectionRest>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Movie.Title));
            CreateMap<PostProjectionRest, Projection>();
            CreateMap<PutProjectionRest, Projection>();

            CreateMap<PostProjectionHallRest, ProjectionHall>();
        }
    }
}