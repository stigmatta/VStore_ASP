using AutoMapper;
using Data_Access.Models;
using Data_Transfer_Object.DTO.User;

namespace Business_Logic.Mappers.UserProfile
{
    public class RegistrationProfile : Profile
    {
        public RegistrationProfile()
        {
            CreateMap<RegistrationDTO, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => GetUsernameFromEmail(src.Email)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.UserAchievements, opt => opt.Ignore())
                .ForMember(dest => dest.UserGames, opt => opt.Ignore())
                .ForMember(dest => dest.Friends, opt => opt.Ignore())
                .ForMember(dest => dest.BlockedUsers, opt => opt.Ignore())
                .ForMember(dest => dest.Wishlist, opt => opt.Ignore());
        }

        private string GetUsernameFromEmail(string? email)
        {
            return email?.Split('@')[0] ?? string.Empty;
        }
    }
}
