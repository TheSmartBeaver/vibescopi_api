using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeScopyAPI.Models.Enums;

namespace VibeScopyAPI.Models
{
	public class UserLikeProfile
	{
        //TODO: COnfigurer la clé primaire composé

        [ForeignKey(nameof(UserProfile))]
        public string UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }

        [ForeignKey(nameof(LikedPerson))]
        public string LikedPersonId { get; set; }

        public UserProfile LikedPerson { get; set; }

        public RateAction RateAction { get; set; }

        //TODO: Rajouter la date de LIKE !!!
    }
}

