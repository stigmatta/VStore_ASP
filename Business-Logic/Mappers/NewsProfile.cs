using AutoMapper;
using Data_Access.Models;
using Data_Transfer_Object.DTO.NewsDTO;

namespace Business_Logic.Mappers
{
    public class NewsProfile:Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate));
        }
    }
}
