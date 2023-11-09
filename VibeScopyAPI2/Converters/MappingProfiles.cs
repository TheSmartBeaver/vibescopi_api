using System;
using AutoMapper;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Controllers;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Dto;

namespace VibeScopyAPI2.Converters
{
    internal class MappingProfiles : AutoMapper.Profile
    {
        static GeometryFactory geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        public MappingProfiles()
        {
            CreateAllMaps();
        }

        private void CreateAllMaps()
        {
            CreateMap<AnswerQuestionDto, Answer>();
            CreateMap<LaunchedActivity, ActivityDto>();
            CreateMap<ActivityCreateDto, LaunchedActivity>()
                .ForMember(dest => dest.Localisation, opt => opt.MapFrom(x => geometryFactory.CreatePoint(new Coordinate(x.Longitude, x.Lattitude))));
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

