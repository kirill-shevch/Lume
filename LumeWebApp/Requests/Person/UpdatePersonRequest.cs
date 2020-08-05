namespace LumeWebApp.Requests.Person
{
	public class UpdatePersonRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Login { get; set; }
		public byte? Age { get; set; }
		public long? CityId { get; set; }
		public string Token { get; set; }
	}
}