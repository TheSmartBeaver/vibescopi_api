using System.ComponentModel.DataAnnotations.Schema;
using VibeScopyAPI2.Models;

namespace VibeScopyAPI.Models
{
	public class Answer
	{
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }

        public short Value { get; set; }
    }
}

