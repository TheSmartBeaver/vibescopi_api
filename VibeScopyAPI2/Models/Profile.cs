using System.ComponentModel.DataAnnotations;
using VibeScopyAPI2.Models;
using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        [Required]
        public string AuthentUid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<SwipedUser> SwipedProfiles { get; set; }

        public SubscriptionType SubscriptionType { get; set; }

        public string? Langages { get; set; } = default!;

        public ProfileProposition ProfileProposition { get; set; }
    }
}

