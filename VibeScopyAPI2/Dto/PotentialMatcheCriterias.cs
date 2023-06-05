using VibeScopyAPI.Models;

namespace VibeScopyAPI.Dto
{
	public class PotentialMatcheCriteriasDto
	{
		public int Distance { get; set; }

        public ICollection<FilamentDto> Filaments { get; set; }

        public int AgeMin { get; set; }

        public int AgeMax { get; set; }

        public ICollection<Gender> Gender { get; set; }
    }

    public class FilamentDto
    {
        public Guid QuestionId { get; set; }

        public short Answer { get; set; }
    }
}