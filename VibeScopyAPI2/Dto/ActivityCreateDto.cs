using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Dto
{
	public class ActivityCreateDto
	{
		public string Name { get; set; }

        public ActivityCategory ActivityCategory { get; set; }

    }
}

