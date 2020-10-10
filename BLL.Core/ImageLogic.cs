using BLL.Core.Interfaces;
using DAL.AzureStorage.Interfaces;
using DAL.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class ImageLogic : IImageLogic
	{
		private readonly IAzureStorageRepository _imageRepository;
		private readonly IPersonRepository _personRepository;
		public ImageLogic(IAzureStorageRepository imageRepository,
			IPersonRepository personRepository)
		{
			_imageRepository = imageRepository;
			_personRepository = personRepository;
		}

		public async Task<Guid> SaveImage(byte[] content)
		{
			var uid = Guid.NewGuid();
			while (await _imageRepository.CheckImageExistance(uid.ToString()))
			{
				uid = Guid.NewGuid();
			}
			await _imageRepository.AddImage(uid.ToString(), content);
			return uid;
		}

		public async Task<byte[]> GetImage(Guid imageUid)
		{
			return await _imageRepository.GetImage(imageUid.ToString());
		}

		public async Task RemoveImage(Guid imageUid)
		{
			if (await _imageRepository.CheckImageExistance(imageUid.ToString()))
			{
				await _imageRepository.RemoveImage(imageUid.ToString());
			}
		}

		public async Task<byte[]> GetMiniatureImage(Guid imageUid)
		{
			var imageEntity = await _personRepository.GetPersonImage(imageUid);
			return await _imageRepository.GetImage(imageEntity.PersonMiniatureImageContentUid.ToString());
		}
	}
}
