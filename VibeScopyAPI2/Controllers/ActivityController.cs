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
using VibeScopyAPI2.Dto;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace VibeScopyAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : BaseController
    {
        private readonly VibeScopUnitOfWork _context;
        private readonly IMapper _mapper;

        public ActivityController(VibeScopUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private void getSubActivities(ICollection<Activity> activities, HashSet<ActivityCategory> outputedSubActivities)
        {

            foreach (var sub in activities)
            {
                outputedSubActivities.Add(sub.ActivityCategory);
                getSubActivities(sub.SubActivities, outputedSubActivities);
            }
        }

        // GET: api/ProfileProposition
        [HttpPost("GetActivitiesNearby")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivitiesNearby(ActivitySearchCriteria criterias)
        {
            HashSet<ActivityCategory> searchedCategories = new HashSet<ActivityCategory>();
            if (criterias.ActivityType != null)
            {
                searchedCategories.Add((ActivityCategory)criterias.ActivityType);
            }


            
            List<LaunchedActivity> activities = new List<LaunchedActivity>();

            var fbUid = await GetAuthenticateUserAsync();

            //ProfileProposition profile = await _context.ProfilePropositions.SingleAsync(x => x.Id == userId);
            ProfileProposition userProfile = await _context.ProfilePropositions
                .Include(x => x.AnswersFilaments)
                .Include(x => x.User)
                    .ThenInclude(x => x.UsersLiked)
                .SingleAsync(x => x.User.AuthentUid == fbUid);

            var distanceInDegrees = (criterias.Distance + 1) * 9 * 111.32; // Approximate conversion from km to degrees

            var activitiesRequest = _context.LaunchedActivities
                .Include(x => x.Participants)
                .Where(x => x.EventDate > DateTime.Now.ToUniversalTime());

            void AttachLocationRequest()
            {
                activitiesRequest = activitiesRequest.Where(launchedActivity =>
                launchedActivity.Localisation.IsWithinDistance(userProfile.LastLocation, distanceInDegrees));
            }

            if (criterias.ActivityType != null)
            {
                var activityCategory = _context.Activities
                                            .Include(x => x.SubActivities)
                                            .ThenInclude(x => x.SubActivities) //TODO: Vorace en perf ?? Pas parfait car si plus de niveaux...
                                            .ThenInclude(x => x.SubActivities)
                                            .ThenInclude(x => x.SubActivities)
                                            .ThenInclude(x => x.SubActivities)
                                            .Single(x => x.ActivityCategory == criterias.ActivityType);
                getSubActivities(activityCategory.SubActivities, searchedCategories);
                activitiesRequest = activitiesRequest.Where(x => searchedCategories.Contains(x.ActivityCategory));
            }

            if (criterias.ActivityType != null)
            {
                HashSet<ActivityCategory> onlineCategories = new HashSet<ActivityCategory>
                {
                    ActivityCategory.ONLINE
                };

                var activityCategory = _context.Activities
                                            .Include(x => x.SubActivities)
                                            .ThenInclude(x => x.SubActivities) //TODO: Vorace en perf ?? Pas parfait car si plus de niveaux...
                                            .ThenInclude(x => x.SubActivities)
                                            .ThenInclude(x => x.SubActivities)
                                            .ThenInclude(x => x.SubActivities)
                                            .Single(x => x.ActivityCategory == ActivityCategory.ONLINE);

                getSubActivities(activityCategory.SubActivities, onlineCategories);

                if (!onlineCategories.Contains((ActivityCategory)criterias.ActivityType))
                {
                    AttachLocationRequest();
                }
                
            } else
            {
                AttachLocationRequest();
            }

            if (!string.IsNullOrEmpty(criterias.Name)) {
                activitiesRequest = activitiesRequest.Where(x => EF.Functions.ILike(x.Name, $"%{criterias.Name}%"));
            }

            activities.AddRange(await activitiesRequest.ToListAsync());

            ICollection<ActivityDto> result = new List<ActivityDto>();

            foreach(var activity in activities)
            {
                var titi = _mapper.Map<ActivityDto>(activity);
                if(activity.Localisation != null)
                {
                    titi.Distance = CalculateDistance(activity.Localisation, userProfile.LastLocation);
                }
                result.Add(titi);
            }

            return Ok(result);
        }

        // POST: api/ProfileProposition
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateActivity")]
        public async Task<ActionResult> PostActivity(ActivityCreateDto activityCreateDto)
        {

            _context.LaunchedActivities.Add(_mapper.Map<LaunchedActivity>(activityCreateDto));
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("UpdateActivity")]
        public async Task<ActionResult> UpdateActivity(ActivityUpdateDto activityUpdateDto)
        {

            _context.Activities.Add(_mapper.Map<Activity>(activityUpdateDto));
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
