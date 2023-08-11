using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Dto;
using VibeScopyAPI2.Models.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VibeScopyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController : ControllerBase
    {
        private readonly VibeScopUnitOfWork _context;
        private readonly IMapper _mapper;

        public MatchingController(VibeScopUnitOfWork context)
        {
            _context = context;
        }

        //Trouver les matchs potentiels
        [HttpPost("getPotentialMatches")]
        public async Task<PotentialMatchDto> getPotentialMatches(PotentialMatcheCriteriasDto potentialMatcheCriterias)
        {
            //Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Guid userId = Guid.Parse("4ca0892b-1ecc-4707-bd5c-d964a01aaa1a");

            //ProfileProposition profile = await _context.ProfilePropositions.SingleAsync(x => x.Id == userId);
            ProfileProposition profile = await _context.ProfilePropositions
                .Include(x => x.AnswersFilaments)
                .SingleAsync(x => x.Id == userId);

            var distanceInDegrees = potentialMatcheCriterias.Distance / 111.32; // Approximate conversion from km to degrees
            var userLocation = new Point(new Coordinate(profile.LastLocation.X, profile.LastLocation.Y)) { SRID = 4326 };
            var nearbyUsers = _context.ProfilePropositions
                .Where(profile => profile.LastLocation.IsWithinDistance(userLocation, distanceInDegrees))
                .AddFilaments(profile.AnswersFilaments)
                //.Include(x => x.User)
                .ToList();

            return new PotentialMatchDto();
        }

        [HttpPost("answerFilamentQuestions/{answersFilamentId}")]
        public async Task<ActionResult> AnswerFilamentQuestions(Guid answersFilamentId, ICollection<AnswerQuestionDto> answerQuestionDtos)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _context.AnswersFilaments.Add(new AnswersFilament()
            {
                ProfileId = userId,
                QuestionFilamentId = answersFilamentId,
                LastUpdate = DateTime.Now,
                Answers = _mapper.Map<ICollection<Answer>>(answerQuestionDtos),
                FilamentValue = "A FAIRE" //TODO: Faire update du filament
            });

            return Ok();
        }

        /*[HttpPost("VIPGetAnswersThatMatters/{profileId}")]
        public async Task<ActionResult<List<ActivityName>>> AnswersThatMatters(Guid profileId, ICollection<Guid> questionIds)
        {
            ICollection<AnswerQuestionDto> answersDto = await _context.AnswersFilaments
                            .Where(x => x.ProfileId == profileId && questionIds.Contains(x.QuestionFilamentId))
                            .ProjectTo<AnswerQuestionDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();

            return Ok(answersDto);
        }*/
    }
}

