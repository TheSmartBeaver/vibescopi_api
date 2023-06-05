using System.ComponentModel.DataAnnotations;

namespace VibeScopyAPI.Models
{
	public class Profile
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public ICollection<AnswersFilament> AnsweredFilament { get; set; }

        public ICollection<Photo> Photos { get; set; }

		public ICollection<Profile> LikedUsers { get; set; }

    }
}

