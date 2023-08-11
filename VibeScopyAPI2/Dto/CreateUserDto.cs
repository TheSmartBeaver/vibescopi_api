using System.ComponentModel.DataAnnotations;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Models;
using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI.Controllers
{
	public class CreateUserDto
	{
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public SubscriptionType SubscriptionType { get; set; }
    }
}

