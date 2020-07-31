using Microsoft.AspNetCore.Http;
using Utils;

namespace BLL.Core
{
	public class BaseValidator
	{
		protected readonly string _culture;
		public BaseValidator(IHttpContextAccessor contextAccessor)
		{
			var httpContext = contextAccessor.HttpContext;
			_culture = CultureParser.GetCultureFromHttpContext(httpContext);
		}
	}
}
