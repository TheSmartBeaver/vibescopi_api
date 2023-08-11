using System;
namespace VibeScopyAPI2.Models
{
	public class ProfileCustomQuestion
	{
		public Guid QuestionId { get; set; }

		public string Description { get; set; }

        public string Answer { get; set; }

        public ICollection<PossibleAnswer> PossibleAnswers { get; set; }

    }
}

