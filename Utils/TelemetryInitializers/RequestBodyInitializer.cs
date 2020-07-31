using Constants;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Linq;

namespace Utils.TelemetryInitializers
{
	public class RequestBodyInitializer : ITelemetryInitializer
    {
        const string userUid = "UserUid";

        private readonly IHttpContextAccessor _httpContextAccessor;
		public RequestBodyInitializer(IHttpContextAccessor httpContextAccessor)
		{
            _httpContextAccessor = httpContextAccessor;
		}
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is RequestTelemetry requestTelemetry)
            {
                StringValues personUid;
                if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthorizationHeaders.PersonUid, out personUid))
                {
                    requestTelemetry.Properties.Add(userUid, personUid.FirstOrDefault());
                }
            }
        }
    }
}
