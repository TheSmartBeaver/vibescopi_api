using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Dto
{
	public class ActivityDto
	{
        public string Name { get; set; }

        public ActivityCategory ActivityCategory { get; set; }

        public double Distance { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public short? MaxParticipants { get; set; }

        public short? MinParticipants { get; set; }

        public short? MinAge { get; set; }

        public short? MaxAge { get; set; }

        public DateTime EventDate { get; set; }

        public ProfileDto Creator { get; set; }

        public ICollection<ProfileDto> Participants { get; set; }
    }
}

