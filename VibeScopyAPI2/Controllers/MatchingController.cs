using System.Security.Claims;
using AutoMapper;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;
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
            ProfileProposition userProfile = await _context.ProfilePropositions
                .Include(x => x.AnswersFilaments)
                .Include(x => x.User)
                    .ThenInclude(x => x.UsersLiked)
                .SingleAsync(x => x.User.AuthentUid == fbUid);

            //TODO: Problème avec les distances...
            //var distanceInDegrees = potentialMatcheCriterias.Distance / 111.32; // Approximate conversion from km to degrees
            var distanceInDegrees = (potentialMatcheCriterias.Distance + 1) * 9 * 111.32; // Approximate conversion from km to degrees
            //var userLocation = new Point(new Coordinate(userProfile.LastLocation.X, userProfile.LastLocation.Y)) { SRID = 4326 };
            List<ProfileProposition> nearbyUsers = _context.ProfilePropositions
                .Where(nearbyUser =>
                 nearbyUser.LastLocation.IsWithinDistance(userProfile.LastLocation, distanceInDegrees))
                .Include(x => x.User)
                .AddFilaments(userProfile.AnswersFilaments)
                .ToList();

            nearbyUsers = nearbyUsers.Where(x => x.User.AuthentUid != fbUid
            && !userProfile.User.UsersLiked.Any(y => y.LikedPersonId == x.User.AuthentUid)).ToList();
            //ICollection<PotentialMatchDto> result = _mapper.Map<ICollection<PotentialMatchDto>>(nearbyUsers);
            ICollection<PotentialMatchDto> result = new List<PotentialMatchDto>();

            foreach (var toto in nearbyUsers)
            {
                if (userProfile.LastLocation.SRID != 4326 || toto.LastLocation.SRID != 4326)
                {
                    throw new ArgumentException("Les points doivent avoir un SRID de 4326.");
                }

                // Calculez la distance en degrés
                double distanceDegrees = userProfile.LastLocation.Distance(toto.LastLocation);

                // Convertissez la distance en degrés en distance en kilomètres.
                // Une approximation commune est d'utiliser 111.32 km comme la distance représentée par 1 degré.
                double distanceKm = distanceDegrees * 111.32 * 0.75;
                var ooo = _mapper.Map<PotentialMatchDto>(toto);
                ooo.Distance = distanceKm;
                result.Add(ooo);
            }

            return Ok(result);
        }

        [HttpPost("getProfilesLikingUs")]
        public async Task<ActionResult<QgDto>> getProfilesLikingUs()
        {
            var fbUid = await GetAuthenticateUserAsync();

            ProfileProposition myProfile = await _context.ProfilePropositions
                .Include(x => x.User)
                .ThenInclude(x => x.UsersLiked)
                .SingleAsync(x => x.User.AuthentUid == fbUid);

            List<ProfileProposition> profilesLikingUs = _context.ProfilePropositions
                //.Where(profile => myProfile.User.UsersLiked.Any(x => x.UserProfile == profile.User))
                .Include(x => x.User)
                .ThenInclude(x => x.UsersLiked)
                .ToList();

            profilesLikingUs = profilesLikingUs.Where(x => x.User.AuthentUid != fbUid).ToList();
            var profilesLikingUsWithoutMatch = profilesLikingUs.Where(x => !myProfile.User.UsersLiked.Any(y => y.UserProfile == x.User && y.RateAction == RateAction.LIKE));

            QgDto qgDto = new QgDto() {
                ProfileLikingUs = _mapper.Map<ICollection<PotentialMatchDto>>(profilesLikingUsWithoutMatch),
                ProfileMatchingUs = _mapper.Map<ICollection<PotentialMatchDto>>(profilesLikingUs.Except(profilesLikingUsWithoutMatch))
            };
            return Ok(qgDto);
        }

        [HttpPost("rateProfile")]
        public async Task<IActionResult> rateProfile(string likedProfileId, RateAction rateAction)
        {
            var fbUid = await GetAuthenticateUserAsync();

            _context.UserLikeProfiles.Add(new UserLikeProfile()
            {
                UserProfileId = fbUid,
                LikedPersonId = likedProfileId,
                RateAction = rateAction
            });

            await _context.SaveChangesAsync();

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("Properties/googleServiceAccountKey.json")
                    //.CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                });
            }

            var message = new FirebaseAdmin.Messaging.Message
            {
                Notification = new Notification
                {
                    Title = "Notif test de Totor",
                    Body = "Je suis un test :)"
                },
                Token = "fYwE_OPyQCK6lK1i62c-AW:APA91bHonFZZlqooRnXvcGLloxcd7mFb5ygLqsjf0OSPZ8tjWn81rWouuzDTmRCdgPJVskeClQySBvA-2SWNH04Hh_OLo3yl7Uska4N8weU8YikoateR-dvuhs0rR_7pxbQBO-KiGtCl" // Le token du dispositif cible
            };

            // Envoi du message
            var messaging = FirebaseMessaging.DefaultInstance;
            var response = await messaging.SendAsync(message);

            return Ok();
        }

        [HttpPost("answerFilamentQuestions/{answersFilamentId}")]
        public async Task<ActionResult> AnswerFilamentQuestions(Guid answersFilamentId, ICollection<AnswerQuestionDto> answerQuestionDtos)
        {
            var fbUid = await GetAuthenticateUserAsync();

            _context.AnswersFilaments.Add(new AnswersFilament()
            {
                ProfileId = fbUid,
                QuestionFilamentId = answersFilamentId,
                LastUpdate = DateTime.Now,
                Answers = _mapper.Map<ICollection<Answer>>(answerQuestionDtos),
                FilamentValue = "A FAIRE" //TODO: Faire update du filament
            });

            await _context.SaveChangesAsync();

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

