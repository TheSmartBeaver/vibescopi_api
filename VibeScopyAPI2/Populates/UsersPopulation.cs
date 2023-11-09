using System;
using VibeScopyAPI.Controllers;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Populates
{
    public class UsersPopulation
    {
        public static List<CreateUserDto> Users = new List<CreateUserDto>() {
                new CreateUserDto()
                {
                      AuthentUid = "ssIEY7b6xTQucmGKhhPBcJ9uVaQ2",
                      Name = "Totor",
                      Email = "totorx36bhu@gmail.com",
                      Phone = "+33695150667",
                      BirthDay = DateTime.Parse("1997-02-17T12:05:51.968Z"),
                      Gender = Gender.HOMME,
                      LovingGenders = new List<Gender> {
                        Gender.FEMME
                      },
                      FriendGenders = new List<Gender> {
                        Gender.FEMME
                      },
                      LookingRelationShips = new List<RelationShip> {
                        RelationShip.FRIENDS,
                        RelationShip.SHORT_TERM
                      }
                },
                new CreateUserDto()
                {
                      AuthentUid = "UhNAi3wdSQgHJcPNLaP5E09YaiW2",
                      Name = "Brigitte Macaron",
                      Email = "totor5dgav@gmail.com",
                      Phone = "+33695150667",
                      BirthDay = DateTime.Parse("1945-08-14T12:05:51.968Z"),
                      Gender = Gender.HOMME,
                      LovingGenders = new List<Gender> {
                        Gender.HOMME
                      },
                      FriendGenders = new List<Gender> {
                        Gender.HOMME
                      },
                      LookingRelationShips = new List<RelationShip> {
                        RelationShip.FRIENDS,
                        RelationShip.SHORT_TERM,
                        RelationShip.LONG_TERM,
                        RelationShip.SERIOUS
                      }
                },
            };


        public static Dictionary<string, UpdateUserLocationDto> UserPositions = new Dictionary<string, UpdateUserLocationDto>()
        {
            { "ssIEY7b6xTQucmGKhhPBcJ9uVaQ2", new UpdateUserLocationDto(){ Latitude = 44.54330674706713, Longitude = 6.131617324086766 } },
            { "UhNAi3wdSQgHJcPNLaP5E09YaiW2", new UpdateUserLocationDto(){ Latitude = 44.566354082987175, Longitude = 6.074984864222871 } },
        };
    }
}

