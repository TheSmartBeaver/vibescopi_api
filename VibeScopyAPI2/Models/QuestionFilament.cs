using System;
namespace VibeScopyAPI2.Models
{
	public class QuestionFilament
	{
		public Guid Id { get; set; }

        public string Description { get; set; }

        public ICollection<PossibleAnswer> PossibleAnswers { get; set; }
    }
}

