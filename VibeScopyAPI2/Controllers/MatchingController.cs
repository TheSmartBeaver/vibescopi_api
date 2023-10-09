using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VibeScopyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController : BaseController
    {
        private readonly VibeScopUnitOfWork _context;
        private readonly IMapper _mapper;

        public MatchingController(VibeScopUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Trouver les matchs potentiels
        [HttpPost("getPotentialMatches")]
        public async Task<ActionResult<ICollection<PotentialMatchDto>>> getPotentialMatches(PotentialMatcheCriteriasDto potentialMatcheCriterias)
        {
            var fbUid = await GetAuthenticateUserAsync();

            //ProfileProposition profile = await _context.ProfilePropositions.SingleAsync(x => x.Id == userId);
            ProfileProposition profile = await _context.ProfilePropositions
                .Include(x => x.AnswersFilaments)
                .Include(x => x.User)
                .SingleAsync(x => x.User.AuthentUid == fbUid);

            var distanceInDegrees = potentialMatcheCriterias.Distance / 111.32; // Approximate conversion from km to degrees
            var userLocation = new Point(new Coordinate(profile.LastLocation.X, profile.LastLocation.Y)) { SRID = 4326 };
            List<ProfileProposition> nearbyUsers = _context.ProfilePropositions
                .Where(profile => profile.LastLocation.IsWithinDistance(userLocation, distanceInDegrees))
                .Include(x => x.User)
                .AddFilaments(profile.AnswersFilaments)
                .ToList();

            ICollection<PotentialMatchDto> result = _mapper.Map<ICollection<PotentialMatchDto>>(nearbyUsers);
            return Ok(result);
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

