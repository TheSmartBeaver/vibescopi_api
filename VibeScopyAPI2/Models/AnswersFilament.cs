namespace VibeScopyAPI.Models
{
	public class AnswersFilament
	{
        public Guid Id { get; set; }

        public string FilamentName { get; set; }

        public string FilamentValue { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}

