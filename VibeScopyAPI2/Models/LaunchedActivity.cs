using System;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Models
{
	public class LaunchedActivity
	{
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ActivityCategory ActivityCategory { get; set; }

        [ForeignKey(nameof(Creator))]
        public string CreatorAuthentUid { get; set; }

        public UserProfile Creator { get; set; }

        public short? MaxParticipants { get; set; }

        public short? MinParticipants { get; set; }

        public string? AccessConditions { get; set; }

        public short? MinAge { get; set; }

        public short? MaxAge { get; set; }

        public ICollection<Gender>? Gender { get; set; }

        public Point? Localisation { get; set; }

        public LevelRequired? LevelRequired { get; set; }
        
        public DateTime EventDate { get; set; } // Pour la périodicité, il faudra renouveler manuellement sur l'appli

        public ICollection<UserProfile> Participants { get; set; } = default!;
    }
}

