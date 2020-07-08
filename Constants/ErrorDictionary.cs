using System.Collections.Generic;

namespace Constants
{
	public static class ErrorDictionary
	{
		public static Dictionary<int, string> Errors = new Dictionary<int, string>
		{
			[1] = "User already exists in the database.",
			[2] = "User is not exist in the database.",
			[3] = "SMS code is not correct.",
			[4] = "Refresh token is invalid",
			[5] = "Authorization failed."
		};

		public static string GetErrorMessage(int code) => $"{{ \"ErrorCode\":{code}, \"Message\":\"{Errors[code]}\" }}";
	}
}
