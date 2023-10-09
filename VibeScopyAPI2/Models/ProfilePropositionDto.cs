using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeScopyAPI2.Models;
using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI.Models
{
    public class ProfilePropositionDto
	{

        public ICollection<AnswersFilament> AnswersFilaments { get; set; }

        public bool IsVerified { get; set; }

        public int Height { get; set; }

        public string? Hobbies { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }

        public ICollection<ProfileCustomQuestion> ProfileCustomQuestions;

        public ICollection<ActivityName> WantedActivities;
    }
}

