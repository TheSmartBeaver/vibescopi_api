using System;
namespace VibeScopyAPI.Models
{
	public class RejectedProfile
	{
		public UserProfile Profile { get; set; }

        public UserProfile Rejected { get; set; }

        public DateTime RejectionDate { get; set; }
    }
}

