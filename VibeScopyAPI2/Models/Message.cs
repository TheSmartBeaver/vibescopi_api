namespace VibeScopyAPI.Models
{
	public class Message
	{
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public Profile Sender { get; set; }

        public Profile Receiver { get; set; }

        public string Value { get; set; }
    }
}

