using AutoMapper;
using BLL.DTO.Developer;
using BLL.DTO.Game;
using BLL.DTO.Genre;
using DAL.Entities;

namespace BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameEntity, GameDTO>().ReverseMap();
            CreateMap<DeveloperEntity, DeveloperDTO>().ReverseMap();
            CreateMap<GenreEntity, GenreDTO>().ReverseMap();
        }
    }
}
