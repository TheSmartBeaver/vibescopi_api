using System;
namespace VibeScopyAPI.Models
{
	public class RejectedProfile
	{
		public Profile Profile { get; set; }

        public Profile Rejected { get; set; }

        public DateTime RejectionDate { get; set; }
    }
}

