using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Dto
{
	public class ActivityCreateDto
	{
		public string Name { get; set; }

        public ActivityCategory ActivityCategory { get; set; }

		public int? MaxParticipants { get; set; }

        public int? MinParticipants { get; set; }

		public string AccessConditions { get; set; }

		public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

		public ICollection<Gender> Gender { get; set; }

		public double lattitude { get; set; }

        public double longitude { get; set; }

		public LevelRequired LevelRequired { get; set; }

        public DateTime EventDate { get; set; }
    }
}

