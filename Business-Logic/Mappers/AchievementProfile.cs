﻿using AutoMapper;
using Data_Access.Models;
using Data_Transfer_Object.DTO.Achievement;
using Data_Transfer_Object.DTO.AdminAchievement;

namespace Business_Logic.Mappers
{
    public class AchievementProfile:Profile
    {
        public AchievementProfile()
        {
            CreateMap<Achievement,AdminAchievementDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo));

            CreateMap<Achievement, AchievementDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo));
        }

    }
}
