using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Models
{
	public class Activity
	{
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ActivityCategory ActivityCategory { get; set; }
    }
}

