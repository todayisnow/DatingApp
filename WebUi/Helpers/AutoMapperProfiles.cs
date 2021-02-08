using System;
using System.Linq;
using WebUi.Dto;
using WebUi.Entities;
using WebUi.Extensions;
using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace WebUi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            //CreateMap<RegisterDto, AppUser>();
            //CreateMap<Message, MessageDto>()
            //    .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => 
            //        src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            //    .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => 
            //        src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}