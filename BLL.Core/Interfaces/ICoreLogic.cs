using System;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface ICoreLogic
	{
		Task CreatePerson(Guid personUid);
	}
}
