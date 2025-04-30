using AutoMapper;
using Data_Transfer_Object.DTO.Game;

namespace Business_Logic.Mappers
{
    public class GameProfile:Profile
    {
        public GameProfile() {
            CreateMap<Game, MainPageGameDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                .ForMember(dest => dest.LogoLink, opt => opt.MapFrom(src => src.Logo))
                .ForMember(dest => dest.Developer, opt => opt.MapFrom(src => src.Developer))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate));
        }
    }
}
