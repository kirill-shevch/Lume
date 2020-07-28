namespace DAL.Core.Models
{
	public class RepositoryGetPersonListFilter
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public string Query { get; set; }
		public long? CityId { get; set; }
	}
}
