namespace VibeScopyAPI.Models
{
	public class Answer
	{
        public Guid Id { get; set; }

        public Question Question { get; set; }

        public short Value { get; set; }
    }
}

