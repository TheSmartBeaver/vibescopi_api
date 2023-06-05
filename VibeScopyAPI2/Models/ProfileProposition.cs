using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace VibeScopyAPI.Models
{
    public class ProfileProposition
    {
        public Guid Id { get; set; }

        public Profile User { get; set; }

        public Point Location { get; set; }

        public ICollection<AnswersFilament> AnswersFilaments { get; set; }
    }
}

