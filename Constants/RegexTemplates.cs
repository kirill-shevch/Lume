namespace Constants
{
	public static class RegexTemplates
	{
		public const string PhoneTemplate = @"^[+]*[0-9]{0,3}[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";
		public const string AuthorizationCodeTemplate = @"^[0-9]{6}$";
	}
}