using System.ComponentModel.DataAnnotations;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Models
{
	public class Activity
	{
        [Key]
        public ActivityCategory ActivityCategory { get; set; }

        public ICollection<Activity> SubActivities { get; set; }
    }
}

