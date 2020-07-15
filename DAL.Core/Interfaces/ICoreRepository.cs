using DAL.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface ICoreRepository
	{
		Task<ChatEntity> GetChat(int id, CancellationToken cancellationToken = default);
		Task<PersonEntity> GetPerson(int id, CancellationToken cancellationToken = default);
		Task<EventEntity> GetEvent(int id, CancellationToken cancellationToken = default);
	}
}