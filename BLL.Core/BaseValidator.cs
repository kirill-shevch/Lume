using Microsoft.AspNetCore.Http;

namespace BLL.Core
{
	public class BaseValidator
	{
		protected readonly string _culture;
		public BaseValidator(IHttpContextAccessor contextAccessor)
		{
			var httpContext = contextAccessor.HttpContext;
			var userLangs = httpContext.Request.Headers["Accept-Language"].ToString();
			if (userLangs.Contains("ru-RU"))
			{
				_culture = "ru-RU";
			}
			else
			{
				_culture = "en-US";
			}
		}
	}
}
