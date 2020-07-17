using BLL.Core.Models.Image;
using System;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IImageLogic
	{
		Task<Guid> SavePersonImage(AddPersonImageModel addPersonImageModel, Guid personUid);
		Task<byte[]> GetPersonImage(Guid imageUid);
	}
}