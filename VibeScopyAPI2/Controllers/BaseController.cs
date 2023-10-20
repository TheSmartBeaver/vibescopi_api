using System;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace VibeScopyAPI.Controllers
{
    public class BaseController : ControllerBase
    {
		public BaseController()
		{
		}

        static SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }

        protected async Task<string> GetAuthenticateUserAsync()
        {
            string idToken = "";
            if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                idToken = HttpContext.Request.Headers["Authorization"].ToString();
            }
            else
            {
                //throw new Exception("No Authorization Header Found !");
                return "ssIEY7b6xTQucmGKhhPBcJ9uVaQ2";
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");
            var response = await client.GetAsync("x509/securetoken@system.gserviceaccount.com");
            if (!response.IsSuccessStatusCode) { throw new Exception(" --- Error trying to retrieve IssuerSigning keys !"); }
            //var x509Data = await response.Content.ReadAsAsync<Dictionary<string, string>>();
            var x509Data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            var decodedToken = tokenHandler.ReadJwtToken(idToken);
            //https://securetoken.google.com/vibescopy
            // Configuration pour décoder le JWT
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // Vous pouvez configurer ces options en fonction de votre cas d'utilisation
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://securetoken.google.com/vibescopy", // Remplacez par votre émetteur (issuer) JWT
                ValidAudience = "vibescopy", // Remplacez par l'ID de votre projet Firebase
                IssuerSigningKeys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray()
            };

            try
            {
                SecurityToken validatedToken;
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(idToken, validationParameters, out validatedToken);
                return decodedToken.Subject;
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.AuthErrorCode == AuthErrorCode.RevokedIdToken)
                {
                    Console.WriteLine("Token expired");
                }
                else
                {
                    var msg = $"Error JWT : {ex.Message}";
                    Console.WriteLine(msg);
                }
                throw ex;
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de décodage ici
                var msg = $"Erreur de décodage JWT : {ex.Message}";
                Console.WriteLine(msg);
                throw ex;
            }

        }
    }
}

