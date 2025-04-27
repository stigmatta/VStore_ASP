using AutoMapper;
using Data_Access.Models;
using Data_Transfer_Object.DTO;

namespace Business_Logic.Mappers
{
    public class RequirementProfile:Profile
    {
        public RequirementProfile()
        {
            CreateMap<RecommendedRequirementDTO, RecommendedRequirement>()
                .ForMember(dest => dest.OS, opt => opt.MapFrom(src => src.OS))
                .ForMember(dest => dest.Processor, opt => opt.MapFrom(src => src.Processor))
                .ForMember(dest => dest.Memory, opt => opt.MapFrom(src => src.Memory))
                .ForMember(dest => dest.Graphics, opt => opt.MapFrom(src => src.Graphics))
                .ForMember(dest => dest.Storage, opt => opt.MapFrom(src => src.Storage))
                .ForMember(dest => dest.Device, opt => opt.MapFrom(src => src.Device));
            CreateMap<MinimumRequirementDTO, MinimumRequirement>()
                .ForMember(dest => dest.OS, opt => opt.MapFrom(src => src.OS))
                .ForMember(dest => dest.Processor, opt => opt.MapFrom(src => src.Processor))
                .ForMember(dest => dest.Memory, opt => opt.MapFrom(src => src.Memory))
                .ForMember(dest => dest.Graphics, opt => opt.MapFrom(src => src.Graphics))
                .ForMember(dest => dest.Storage, opt => opt.MapFrom(src => src.Storage))
                .ForMember(dest => dest.Device, opt => opt.MapFrom(src => src.Device));
        }
    }
}
