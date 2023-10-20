using System.ComponentModel.DataAnnotations.Schema;
using VibeScopyAPI2.Models;

namespace VibeScopyAPI.Models
{
	public class AnswersFilament
	{
        public Guid Id { get; set; }

        public string ProfileId { get; set; }

        public Guid QuestionFilamentId { get; set; }

        [ForeignKey(nameof(QuestionFilamentId))]
        public QuestionFilament QuestionFilament { get; set; }

        public string FilamentValue { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public DateTime LastUpdate { get; set; }

    }
}

