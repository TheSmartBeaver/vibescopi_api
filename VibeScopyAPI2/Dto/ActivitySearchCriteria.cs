using System;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Dto
{
	public class ActivitySearchCriteria
	{
        public string Name { get; set; }

        public ActivityCategory? ActivityType { get; set; }

        public int Distance { get; set; }
    }
}

