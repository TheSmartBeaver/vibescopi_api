using System;
using System.Security.Claims;
using VibeScopyAPI2.Dto;
using VibeScopyAPI2.Models.Enums;

namespace VibeScopyAPI2.AppServices
{
	public class AuthorizationAppService
	{
		public AuthorizationAppService()
		{

		}

		public ProfileAuthorizationsDto AvailableAuthorizations(ClaimsPrincipal user)
		{
			return new ProfileAuthorizationsDto()
			{
				CanSwipeBack = CanSwipeBack(user)
			};
        }

		private bool CanSwipeBack(ClaimsPrincipal user)
		{
			SubscriptionType subscriptionType = Enum.Parse<SubscriptionType>(user.FindFirst(CustomClaimTypes.SubscriptionType).Value);
			switch (subscriptionType)
			{
				case SubscriptionType.PLATINUM:
				case SubscriptionType.GOLD:
                case SubscriptionType.BASIC:
					return true;
				default:
					return false;
			}
		}
    }
}

