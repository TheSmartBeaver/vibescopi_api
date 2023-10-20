using System.ComponentModel.DataAnnotations;
using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI.Dto
{
	public class PotentialMatchDto
	{
        [Required]
        public string AuthentUid { get; set; }

        [Required]
        public string Name { get; set; }

		public double? Distance { get; set; }

		public string Bio { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }

		public int MatchingRate { get; set; }

		public ICollection<ActivityName> ResearchedActivities { get; set; }
	}
}

