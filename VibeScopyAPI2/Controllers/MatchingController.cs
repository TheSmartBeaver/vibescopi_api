using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VibeScopyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController : ControllerBase
    {
        private readonly UnitOfWorkToto _context;

        public MatchingController(UnitOfWorkToto context)
        {
            _context = context;
        }

        //Trouver les matchs potentiels
        [HttpPost("getPotentialMatches")]
        public async Task<PotentialMatchDto> getPotentialMatches(PotentialMatcheCriteriasDto potentialMatcheCriterias)
        {
            //Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Guid userId = new Guid();

            ProfileProposition profile = await _context.ProfilePropositions.SingleAsync(x => x.Id == userId);

            var distanceInDegrees = potentialMatcheCriterias.Distance / 111.32; // Approximate conversion from km to degrees
            var userLocation = new Point(profile.Location.X, profile.Location.Y) { SRID = 4326 };
            var nearbyUsers = _context.ProfilePropositions
                .Where(profile => profile.Location.IsWithinDistance(userLocation, distanceInDegrees))
                .AddFilaments(profile.AnswersFilaments)
                .Include(x => x.User)
                .ToList();

            return new PotentialMatchDto();
        }

    }
}

