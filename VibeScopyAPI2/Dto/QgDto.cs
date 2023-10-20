using System.ComponentModel.DataAnnotations;

namespace VibeScopyAPI.Dto
{
	public class QgDto
	{
		public ICollection<PotentialMatchDto> ProfileLikingUs { get; set; }

        public ICollection<PotentialMatchDto> ProfileMatchingUs { get; set; }
    }
}

