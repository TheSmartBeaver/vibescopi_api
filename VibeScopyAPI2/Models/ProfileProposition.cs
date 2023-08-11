using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using VibeScopyAPI2.Models;
using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI.Models
{
    public class ProfileProposition
    {
        public Guid Id { get; set; }

        public UserProfile User { get; set; }

        [Column(TypeName = "geography (point)")]
        public Point LastLocation { get; set; }

        public ICollection<AnswersFilament> AnswersFilaments { get; set; }

        public bool IsVerified { get; set; }

        public int Height { get; set; }

        public string Hobbies { get; set; }

        public DateTime BirthDay { get; set; }

        public ICollection<ProfileCustomQuestion> ProfileCustomQuestions;

        public ICollection<ActivityName> WantedActivities;
    }
}

