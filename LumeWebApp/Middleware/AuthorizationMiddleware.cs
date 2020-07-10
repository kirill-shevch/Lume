using BLL.Authorization.Interfaces;
using Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LumeWebApp.Middleware
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class AuthorizationMiddleware
	{
		private readonly RequestDelegate _next;
		//private readonly IAuthorizationLogic _authorizationLogic;

		//public AuthorizationMiddleware(RequestDelegate next, IAuthorizationLogic authorizationLogic)
		//{
		//	_next = next;
		//	_authorizationLogic = authorizationLogic;
		//}

		public AuthorizationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public Task Invoke(HttpContext httpContext)
		{
			//StringValues userUid, accessToken;
			//if (httpContext.Request.Headers.TryGetValue(AuthorizationHeaders.UserUid, out userUid) &&
			//	httpContext.Request.Headers.TryGetValue(AuthorizationHeaders.AccessToken, out accessToken))
			//{
			//	if (!_authorizationLogic.CheckAccessKey(new Guid(userUid.FirstOrDefault()), accessToken.FirstOrDefault()).Result)
			//	{
			//		httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			//		httpContext.Response.ContentType = "application/json";
			//		return httpContext.Response.WriteAsync(ErrorDictionary.GetErrorMessage(5));
			//	}
			//}
			//else
			//{
			//	httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			//	httpContext.Response.ContentType = "application/json";
			//	return httpContext.Response.WriteAsync(ErrorDictionary.GetErrorMessage(5));
			//}
			return _next(httpContext);
		}
	}
}
