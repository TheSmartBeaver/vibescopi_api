namespace VibeScopyAPI.Models
{
	public class Message
	{
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public UserProfile Sender { get; set; }

        public UserProfile Receiver { get; set; }

        public string Value { get; set; }
    }
}

