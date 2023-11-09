using NetTopologySuite.Geometries;
using Swashbuckle.AspNetCore.SwaggerGen;
using VibeScopyAPI.Controllers;
using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Populates
{
    public class ActivitiesPopulation
    {
        public static List<Activity> Activities = new List<Activity>() {
                new Activity()
                {
                    ActivityCategory = ActivityCategory.ONLINE
                },
                new Activity()
                {
                    ActivityCategory = ActivityCategory.VIDEO_GAME
                },
                new Activity()
                {
                    ActivityCategory = ActivityCategory.FORTNITE
                },
                new Activity()
                {
                    ActivityCategory = ActivityCategory.WARZONE
                },
                new Activity()
                {
                    ActivityCategory = ActivityCategory.SPORT
                },
                new Activity()
                {
                    ActivityCategory = ActivityCategory.ESCALADE
                },
                new Activity()
                {
                    ActivityCategory = ActivityCategory.TENNIS
                },
        };

        public static Dictionary<ActivityCategory, ICollection<ActivityCategory>> SubActivities = new Dictionary<ActivityCategory, ICollection<ActivityCategory>>() {
            { ActivityCategory.ONLINE, new List<ActivityCategory>(){ ActivityCategory.VIDEO_GAME } },
            { ActivityCategory.VIDEO_GAME, new List<ActivityCategory>(){ ActivityCategory.FORTNITE, ActivityCategory.WARZONE } },
            { ActivityCategory.SPORT, new List<ActivityCategory>(){ ActivityCategory.ESCALADE, ActivityCategory.TENNIS } }
        };

        static GeometryFactory geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        public static List<LaunchedActivity> LaunchedActivities = new List<LaunchedActivity>() {
            new LaunchedActivity()
                {
                    Name = "Tennis avec le castor",
                    CreatorAuthentUid = "ssIEY7b6xTQucmGKhhPBcJ9uVaQ2",
                    ActivityCategory = ActivityCategory.TENNIS,
                    Localisation = geometryFactory.CreatePoint(new Coordinate(6.064529263363547, 44.53749505062509)),
                    EventDate = DateTime.Now.AddDays(14).ToUniversalTime()
                },
            new LaunchedActivity()
                {
                    Name = "Fortnite Duo Cash Cup",
                    CreatorAuthentUid = "ssIEY7b6xTQucmGKhhPBcJ9uVaQ2",
                    ActivityCategory = ActivityCategory.FORTNITE,
                    EventDate = DateTime.Now.AddDays(7).ToUniversalTime(),
                    MaxParticipants = 1
                },
            new LaunchedActivity()
                {
                    Name = "Apéro des familles",
                    CreatorAuthentUid = "ssIEY7b6xTQucmGKhhPBcJ9uVaQ2",
                    ActivityCategory = ActivityCategory.PARTY,
                    EventDate = DateTime.Now.AddDays(3).ToUniversalTime(),
                    MaxParticipants = 16
                },
        };
    }

}