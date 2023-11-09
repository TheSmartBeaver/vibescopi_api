using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using System.Security.Cryptography.X509Certificates;
using FirebaseAdmin.Auth;
using System.Linq;

namespace VibeScopyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly VibeScopUnitOfWork _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(VibeScopUnitOfWork context,
            IConfiguration configuration,
            IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("authentificate")]
        public async Task<ActionResult<AuthenticateDto>> Authentificate(AuthentificateEntryDto userForLoginDto)
        {
            AuthenticateDto authenticateDto = new AuthenticateDto();
            var user = await _context.Profiles.FirstOrDefaultAsync(x => x.AuthentUid == userForLoginDto.AuthentUid);
            authenticateDto.IsRegistered = user != null;
            return Ok(authenticateDto);
        }

            [HttpPost("login")]
        public async Task<IActionResult> Login(AuthentificateEntryDto userForLoginDto)
        {
            //var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            Models.UserProfile userFromRepo = await _context.Profiles.SingleAsync(/*x => x.Email == userForLoginDto.Email || x.Phone == userForLoginDto.PhoneNumber*/);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                //new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("Token").ToString()));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

        [HttpPost("firebaseLogin2")]
        public async Task<IActionResult> FirebaseLogin2(AuthentificateEntryDto userForLoginDto)
        {
            //FirebaseApp.Create(new AppOptions
            //{
            //Credential = firebaseCredential,
            //});

            FirebaseApp app = null;
            if (FirebaseApp.DefaultInstance == null)
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("Properties/googleServiceAccountKey.json")
                    //.CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                });
            }

            // Vérifiez le jeton d'authentification reçu du client Flutter
            var token = userForLoginDto.IdToken;
            var firebaseToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            var uid = firebaseToken.Uid;

            // Vérifiez le UID de l'utilisateur dans votre base de données ou effectuez d'autres opérations d'authentification personnalisées.

            return Ok();
        }

        [HttpPost("firebaseLogin")]
        public async Task<IActionResult> FirebaseLogin(AuthentificateEntryDto userForLoginDto)
        {
            return Ok();
        }

        [HttpPost("getUserProfile/{fbId}")]
        public async Task<ActionResult<ProfileDto>> GetUser(string fbId)
        {
            var profile = await _context.Profiles.Include(x => x.ProfileProposition).SingleAsync(x => x.AuthentUid == fbId);
            return Ok(_mapper.Map<ProfileDto>(profile));
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = _mapper.Map<Models.UserProfile>(createUserDto);
                _context.Profiles.Add(user);
                await _context.SaveChangesAsync();

                var profileProposition = _mapper.Map<ProfileProposition>(createUserDto);
                profileProposition.Id = user.AuthentUid;
                _context.ProfilePropositions.Add(profileProposition);
                await _context.SaveChangesAsync();

                var userPreferences = _mapper.Map<UserPreferences>(createUserDto);
                userPreferences.Id = user.AuthentUid;
                _context.UserPreferences.Add(userPreferences);
                await _context.SaveChangesAsync();

                transaction.Commit();

                //TODO: Preferences
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // En cas d'échec, annule la transaction
                throw; // Vous pouvez gérer l'exception ici ou la laisser se propager
            }

            return Ok();
        }

        //TODO: Remove car useless désormais ??
        [HttpPost("updateUserPhoto")]
        public async Task<IActionResult> UpdateUserPhoto(UpdateUserPhotoDto updateUserDto)
        {
            var user = await _context.Profiles.SingleAsync(x => x.AuthentUid == updateUserDto.FbId);
            foreach(var photoPath in updateUserDto.AWSS3Path)
            {
                Photo photo = new Photo() { ProfileId = user.AuthentUid, AWSPathS3 = photoPath };
                _context.Photos.Add(photo);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("updateUserLocation")]
        public async Task<IActionResult> UpdateUserLocation(UpdateUserLocationDto updateUserDto)
        {
            var fbUid = await GetAuthenticateUserAsync();

            ProfileProposition user = await _context.ProfilePropositions.Include(x => x.User).SingleAsync(x => x.User.AuthentUid == fbUid);

            var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var currentLocation = geometryFactory.CreatePoint(new Coordinate(updateUserDto.Longitude, updateUserDto.Latitude));
            user.LastLocation = currentLocation;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("deleteUserPhoto")]
        public async Task<IActionResult> deleteUserPhoto(UpdateUserPhotoDto updateUserDto)
        {
            foreach (var photoPath in updateUserDto.AWSS3Path)
            {

                var photo = await _context.Photos.SingleAsync(x => x.AWSPathS3 == photoPath);
                _context.Photos.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
