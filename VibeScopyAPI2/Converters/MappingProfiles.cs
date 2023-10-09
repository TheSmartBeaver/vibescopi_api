using System;
using AutoMapper;
using VibeScopyAPI.Controllers;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Dto;

namespace VibeScopyAPI2.Converters
{
    internal class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateAllMaps();
        }

        private void CreateAllMaps()
        {
            CreateMap<AnswerQuestionDto, Answer>();

            CreateMap<Activity, ActityDto>();
            CreateMap<ActivityCreateDto, Activity>();
            CreateMap<CreateUserDto, UserProfile>();
            CreateMap<CreateUserDto, UserPreferences>();
            CreateMap<CreateUserDto, ProfileProposition>()
                .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(x => x.BirthDay.ToUniversalTime()));

            CreateMap<UserProfile, ProfileDto>();
            CreateMap<ProfileProposition, ProfilePropositionDto>();
            CreateMap<ProfileProposition, PotentialMatchDto>()
                .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(x => x.BirthDay.ToUniversalTime()))
                .ForMember(dest => dest.AuthentUid, opt => opt.MapFrom(x => x.User.AuthentUid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.User.Name));
        }
    }
}

