using System;
using VibeScopyAPI.Models;

namespace VibeScopyAPI2.Dto
{
	public class ProfilePropositionCreateDto
	{
        public ICollection<AnswersFilament> AnswersFilaments { get; set; }
    }
}

