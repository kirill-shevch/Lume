using DAL.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface IChatRepository
	{
		Task<ChatEntity> GetChat(int id, CancellationToken cancellationToken = default);
	}
}
