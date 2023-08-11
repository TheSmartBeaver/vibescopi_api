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
            CreateMap<CreateUserDto, VibeScopyAPI.Models.UserProfile>();
        }
    }
}

