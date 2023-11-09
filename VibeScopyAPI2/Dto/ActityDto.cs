using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Dto
{
	public class ActivityDto
	{
        public string Name { get; set; }

        public ActivityCategory ActivityCategory { get; set; }

        public double Distance { get; set; }
    }
}

