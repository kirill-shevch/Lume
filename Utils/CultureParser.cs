using Microsoft.AspNetCore.Http;

namespace Utils
{
	public static class CultureParser
	{
		public static string GetCultureFromHttpContext(HttpContext httpContext)
		{
			var userLangs = httpContext.Request.Headers["Accept-Language"].ToString();
			if (userLangs.Contains("ru-RU"))
			{
				return "ru-RU";
			}
			else
			{
				return "en-US";
			}
		}

		public static string GetDefaultCulture()
		{
			return "ru-RU";
		}
	}
}
