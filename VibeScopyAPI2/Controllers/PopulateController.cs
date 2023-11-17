using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Controllers;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;
using VibeScopyAPI.Populates;
using VibeScopyAPI2.Dto;

namespace VibeScopyAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateController : ControllerBase
    {
        private readonly VibeScopUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserController _userController;

        public PopulateController(VibeScopUnitOfWork context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userController = new UserController(context, configuration, mapper);
        }

        

        [HttpPost("populate")]
        public async Task<ActionResult> Populate()
        {
            await PopulateUsersAsync();
            await PopulateActivities();

            return Ok();
        }

        [HttpPost("depopulate")]
        public async Task<ActionResult> DePopulate()
        {
            //TODO: Vider tout

            return Ok();
        }

        private async Task PopulateUsersAsync()
        {

            foreach (var user in UsersPopulation.Users)
            {
                await _userController.CreateUser(user);
                await _context.SaveChangesAsync();
                var userCreated = _context.ProfilePropositions.Single(x => x.Id == user.AuthentUid);
                var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                userCreated.LastLocation = geometryFactory.CreatePoint(new Coordinate(UsersPopulation.UserPositions[user.AuthentUid].Longitude, UsersPopulation.UserPositions[user.AuthentUid].Latitude));
            }
        }

        private async Task PopulateActivities()
        {
            _context.AddRange(ActivitiesPopulation.Activities);

            await _context.SaveChangesAsync();

            Dictionary<ActivityCategory, Activity> activities = new Dictionary<ActivityCategory, Activity>();

            foreach (var activityCategory in ActivitiesPopulation.SubActivities)
            {
                Activity activity = _context.Activities.Single(x => x.ActivityCategory == activityCategory.Key);
                foreach (var subActivity in activityCategory.Value)
                {
                    if (!activities.ContainsKey(subActivity))
                    {
                        activities[subActivity] = _context.Activities.Single(x => x.ActivityCategory == subActivity);
                    }
                }
                activity.SubActivities = activities.Where(x => activityCategory.Value.Contains(x.Key)).Select(x => x.Value).ToList();
                await _context.SaveChangesAsync();
            }

            _context.AddRange(ActivitiesPopulation.LaunchedActivities);
            await _context.SaveChangesAsync();

            var launchedActivity = await _context.LaunchedActivities.SingleAsync(x => x.Name == "Tennis avec le castor");
            UserProfile participant = await _context.Profiles.SingleAsync(x => x.AuthentUid == "UhNAi3wdSQgHJcPNLaP5E09YaiW2");
            launchedActivity.Participants = new List<UserProfile>(){ participant };
            await _context.SaveChangesAsync();
        }
    }
}
