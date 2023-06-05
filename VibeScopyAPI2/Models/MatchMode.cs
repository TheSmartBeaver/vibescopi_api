namespace VibeScopyAPI.Models
{
	public class MatchMode
	{
        public Guid Id { get; set; }

        public MatchModeType MatchModeType { get; set; }

        public Profile User { get; set; }

        public ICollection<Profile> UsersLiking { get; set; }

        public ICollection<Profile> UsersLiked { get; set; }
    }
}

