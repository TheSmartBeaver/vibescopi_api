using System.ComponentModel.DataAnnotations;
using VibeScopyAPI.Models;

namespace VibeScopyAPI.Dto
{
	public class ProfileDto
	{
        [Required]
        public string AuthentUid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ProfilePropositionDto ProfileProposition { get; set; }
    }
}

