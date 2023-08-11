using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Dto;

namespace VibeScopyAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly VibeScopUnitOfWork _context;
        private readonly IMapper _mapper;

        public ActivityController(VibeScopUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ProfileProposition
        [HttpPost("GetActivitiesNearby")]
        public async Task<ActionResult<IEnumerable<ActityDto>>> GetActivitiesNearby(ActivitySearchCriteria criterias)
        {
            var activitiesRequest = _context.Activities.Where(x => x.ActivityCategory == criterias.ActivityType);
            if (!string.IsNullOrEmpty(criterias.Name)) { activitiesRequest = activitiesRequest.Where(x => x.Name.Contains(criterias.Name)); }
            ICollection<Activity> activities = await activitiesRequest.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ActityDto>>(activities));
        }

        // POST: api/ProfileProposition
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateActivity")]
        public async Task<ActionResult> PostActivity(ActivityCreateDto activityCreateDto)
        {

            _context.Activities.Add(_mapper.Map<Activity>(activityCreateDto));
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
