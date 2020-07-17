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
			[4] = "Refresh token is invalid.",
			[5] = "Authorization failed.",
			[6] = "Phone number format is not correct.",
			[7] = "Authorization code format is not correct.",
			[8] = "Refresh token should not be empty.",
			[9] = "Image content should not be empty.",
			[10] = "Event is not exist in the database.",
			[11] = "Chat message is not exist in the database.",
			[12] = "Image with such GUID is not exist."
		};

		public static string GetErrorMessage(int code) => $"{{ \"errorCode\":{code}, \"message\":\"{Errors[code]}\" }}";
	}
}
