using System;
using Microsoft.EntityFrameworkCore;
using VibeScopyAPI.Models;

namespace VibeScopyAPI.Infrastructure
{
	public static class QueryExtensions
	{
		public static IQueryable<ProfileProposition> AddFilaments(this IQueryable<ProfileProposition> profilePropositions, ICollection<AnswersFilament> answersFilaments)
		{
			foreach(var answersFilament in answersFilaments)
			{
                profilePropositions = profilePropositions.Where(profile =>
					EF.Functions.FuzzyStringMatchLevenshteinLessEqual(
						profile.AnswersFilaments.First(
							x => x.QuestionFilamentId == answersFilament.QuestionFilamentId).FilamentValue, answersFilament.FilamentValue, 5) < 5);
            }
			return profilePropositions;
        }
    }
}

