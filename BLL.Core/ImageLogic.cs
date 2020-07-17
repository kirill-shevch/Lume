using BLL.Core.Interfaces;
using BLL.Core.Models.Image;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class ImageLogic : IImageLogic
	{
		private readonly IImageRepository _imageRepository;
		public ImageLogic(IImageRepository imageRepository)
		{
			_imageRepository = imageRepository;
		}

		public async Task<Guid> SavePersonImage(AddPersonImageModel addPersonImageModel, Guid personUid)
		{
			using (var sha256Hash = SHA256.Create())
			{
				var hashInBytes = sha256Hash.ComputeHash(addPersonImageModel.Content);
				var hash = Encoding.UTF8.GetString(hashInBytes, 0, hashInBytes.Length);
				var uid = await _imageRepository.GetPersonImageUidByHash(hash);
				if (!uid.HasValue)
				{
					uid = Guid.NewGuid();
					await _imageRepository.SavePersonImage(personUid, new PersonImageContentEntity
					{
						PersonImageContentUid = uid.Value,
						Content = addPersonImageModel.Content,
						ContentHash = hash
					});
					return uid.Value;
				}
				return uid.Value;
			}
		}

		public async Task<byte[]> GetPersonImage(Guid imageUid)
		{
			return await _imageRepository.GetPersonImageContentByUid(imageUid);
		}
	}
}
