using System.Threading.Tasks;

namespace DAL.AzureStorage.Interfaces
{
	public interface IAzureStorageRepository
	{
		Task AddImage(string name, byte[] content);
		Task<byte[]> GetImage(string name);
		Task<bool> CheckImageExistance(string name);
	}
}
