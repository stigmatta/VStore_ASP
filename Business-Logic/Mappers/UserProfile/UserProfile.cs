using AutoMapper;
using Data_Access.Models;
using Data_Transfer_Object.DTO.UserDTO;

namespace Business_Logic.Mappers.UserProfile
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, ProfileDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo));
        }

    }
}
