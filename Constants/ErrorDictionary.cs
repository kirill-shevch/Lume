using System.Collections.Generic;

namespace Constants
{
	public static class ErrorDictionary
	{
		public static Dictionary<int, string> Errors = new Dictionary<int, string>
		{
			[1] = "Person already exists in the database.",
			[2] = "Person is not exist in the database.",
			[3] = "SMS code is not correct.",
			[4] = "Refresh token is invalid.",
			[5] = "Authorization failed.",
			[6] = "Phone number format is not correct.",
			[7] = "Authorization code format is not correct.",
			[8] = "Refresh token should not be empty.",
			[9] = "Image content should not be empty.",
			[10] = "Event is not exist in the database.",
			[11] = "Chat message is not exist in the database.",
			[12] = "Image with such GUID is not exist.",
			[13] = "Event status id is not correct.",
			[14] = "Event type id is not correct.",
			[15] = "Minimal age should be less or equals to the maximum age.",
			[16] = "Event name is required.",
			[17] = "This person don't have this friend in his friend list.",
			[18] = "This person already have this friend in his friend list.",
			[19] = "Chat message is not exist in the database.",
			[20] = "Person is not a chat member."
		};

		public static string GetErrorMessage(int code) => $"{{ \"errorCode\":{code}, \"message\":\"{Errors[code]}\" }}";
	}
}
