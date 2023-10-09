using System.ComponentModel.DataAnnotations.Schema;

namespace VibeScopyAPI.Models
{
	public class Photo
	{
        public Guid Id { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid ProfileId { get; set; }

        public UserProfile UserProfile { get; set; }

        public string AWSPathS3 { get; set; }
    }
}

