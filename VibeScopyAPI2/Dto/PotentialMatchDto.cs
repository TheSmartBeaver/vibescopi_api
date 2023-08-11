using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI.Dto
{
	public class PotentialMatchDto
	{
		public string Name { get; set; }

		public decimal Distance { get; set; }

		public string Bio { get; set; }

		public DateTime BirthDay { get; set; }

		public int MatchingRate { get; set; }

		public ICollection<ActivityName> ResearchedActivities { get; set; }
	}
}

