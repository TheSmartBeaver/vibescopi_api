using System.ComponentModel.DataAnnotations;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Models
{
	public class UserPreferences
	{
        [Key]
        public string Id { get; set; }

        public ICollection<Gender> LovingGenders { get; set; }

        public ICollection<Gender> FriendGenders { get; set; }

        public ICollection<RelationShip> LookingRelationShips { get; set; }
    }
}

