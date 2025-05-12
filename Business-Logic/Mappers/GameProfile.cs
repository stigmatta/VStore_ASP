using AutoMapper;
using Data_Access.Models;
using Data_Transfer_Object.DTO;
using Data_Transfer_Object.DTO.Game;
using Data_Transfer_Object.DTO.GameDTO;

namespace Business_Logic.Mappers
{
    public class GameProfile:Profile
    {
        public GameProfile() {
            CreateMap<Game, MainPageGameDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                .ForMember(dest => dest.LogoLink, opt => opt.MapFrom(src => src.Logo))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate));
            CreateMap<Game,GameDTO>()
                .ForMember(GameDTO => GameDTO.Id,opt => opt.MapFrom(src=>src.Id))
                .ForMember(GameDTO => GameDTO.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(GameDTO => GameDTO.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(GameDTO => GameDTO.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(GameDTO => GameDTO.Discount, opt => opt.MapFrom(src => src.Discount))
                .ForMember(GameDTO => GameDTO.LogoLink, opt => opt.MapFrom(src => src.Logo))
                .ForMember(GameDTO => GameDTO.Developer, opt => opt.MapFrom(src => src.Developer))
                .ForMember(GameDTO => GameDTO.Publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(GameDTO => GameDTO.PEGI, opt => opt.MapFrom(src => src.PEGI))
                .ForMember(GameDTO => GameDTO.TrailerLink, opt => opt.MapFrom(src => src.TrailerLink))
                .ForMember(GameDTO => GameDTO.RecommendedRequirementId, opt => opt.MapFrom(src => src.RecommendedRequirementId))
                .ForMember(GameDTO => GameDTO.MinimumRequirementId, opt => opt.MapFrom(src => src.MinimumRequirementId))
                .ForMember(GameDTO => GameDTO.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(GameDTO => GameDTO.Gallery, opt => opt.MapFrom(src => src.GameGalleries.Select(x => x.Link).ToList()));

        }
    }
}
