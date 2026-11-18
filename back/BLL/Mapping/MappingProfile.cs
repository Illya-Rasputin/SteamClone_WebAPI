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

            // GameEntity -> GameDTO
            CreateMap<GameEntity, GameDTO>();
            // CreateGameDTO -> GameEntity
            CreateMap<CreateGameDTO, GameEntity>()
                .ForMember(
            destination => destination.Id,
            options => options.Ignore())
                .ForMember(
            destination => destination.Images,
            options => options.Ignore())
                .ForMember(
            destination => destination.Genres,
            options => options.Ignore())
                .ForMember(
            destination => destination.Developer,
            options => options.Ignore());

            CreateMap<UpdateGameDTO, GameEntity>()
                .ForMember(
            destination => destination.Images,
            options => options.Ignore())
                .ForMember(
            destination => destination.Genres,
            options => options.Ignore())
                .ForMember(
            destination => destination.Developer,
            options => options.Ignore());

            // GameImageEntity -> GameImageDTO
            CreateMap<GameImageEntity, GameImageDTO>()
                .ForMember(
                    destination => destination.Game,
                    options => options.Ignore());

            // DeveloperEntity -> DeveloperDto
            CreateMap<DeveloperEntity, DeveloperDTO>();

            // CreateDeveloperDto -> DeveloperEntity
            CreateMap<CreateDeveloperDTO, DeveloperEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // UpdateDeveloperDto -> DeveloperEntity
            CreateMap<UpdateDeveloperDTO, DeveloperEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // GenreEntity -> GenreDto
            CreateMap<GenreEntity, GenreDTO>();

            // CreateGenreDto -> GenreEntity
            CreateMap<CreateGenreDTO, GenreEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // UpdateGenreDto -> GenreEntity
            CreateMap<UpdateGenreDTO, GenreEntity>();
        }
    }
}
