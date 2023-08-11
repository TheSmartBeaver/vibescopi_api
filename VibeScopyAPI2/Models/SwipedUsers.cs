using System;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI2.Models
{
	public class SwipedUser
	{
        public Guid Id { get; set; }

        public UserProfile Profile { get; set; }

        public SwipeStatus SwipeStatus { get; set; }

        public DateTime Date { get; set; }
    }
}

