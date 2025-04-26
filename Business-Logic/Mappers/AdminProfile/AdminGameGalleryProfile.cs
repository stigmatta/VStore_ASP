//using AutoMapper;
//using Data_Access.Models;
//using VStore.DTO.Game;

//namespace Business_Logic.Mappers.AdminProfile
//{
//    public class AdminGameGalleryProfile:Profile
//    {
//        public AdminGameGalleryProfile()
//        {
//            CreateMap<GameGalleryDTO, GameGallery>()
//                .ForMember(dest=>dest.Link, opt => opt.MapFrom(src => src.Link))
//                .ForMember(dest => dest.IsCover, opt => opt.MapFrom(src => src.IsCover))
//                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
//                .ForMember(dest => dest.GameId, opt => opt.Ignore())
//                .ForMember(dest => dest.Game, opt => opt.Ignore());
//        }

//    }
//}
