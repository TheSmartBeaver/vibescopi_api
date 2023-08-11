namespace VibeScopyAPI.Models
{
	public class MatchMode
	{
        public Guid Id { get; set; }

        public MatchModeType MatchModeType { get; set; }

        public UserProfile User { get; set; }

        public ICollection<UserProfile> UsersLiking { get; set; }

        public ICollection<UserProfile> UsersLiked { get; set; }
    }
}

