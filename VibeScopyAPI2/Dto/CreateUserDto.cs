using System.ComponentModel.DataAnnotations;
using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Controllers
{
	public class CreateUserDto
	{
        public string AuthentUid { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public DateTime BirthDay { get; set; }

        public Gender Gender { get; set; }

        public ICollection<Gender> LovingGenders { get; set; }

        public ICollection<Gender> FriendGenders { get; set; }

        public ICollection<RelationShip> LookingRelationShips { get; set; }
    }
}

