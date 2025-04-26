using AutoMapper;
using Data_Access.Models;
using VStore.DTO.User;

namespace Business_Logic.Mappers.UserProfile
{
    public class LoginProfile:Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginDTO, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.IsAdmin, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.UserAchievements, opt => opt.Ignore())
                .ForMember(dest => dest.UserGames, opt => opt.Ignore())
                .ForMember(dest => dest.Friends, opt => opt.Ignore())
                .ForMember(dest => dest.BlockedUsers, opt => opt.Ignore())
                .ForMember(dest => dest.Wishlist, opt => opt.Ignore());
        }
    }
}
