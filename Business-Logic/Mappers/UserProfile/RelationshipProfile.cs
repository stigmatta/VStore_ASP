using AutoMapper;
using Data_Access.Models;
using Data_Transfer_Object.DTO.UserDTO;

namespace Business_Logic.Mappers.UserProfile
{
    public class RelationshipProfile:Profile
    {
        public RelationshipProfile()
        {
            CreateMap<Relationship, ProfileDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FriendId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.FriendUser.Username))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.FriendUser.Level))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.FriendUser.Photo));
        }
    }
}
