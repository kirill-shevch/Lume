using BLL.Core.Interfaces;
using Constants;
using DAL.AzureStorage.Interfaces;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace BLL.Core
{
	public class ImageValidation : BaseValidator, IImageValidation
	{
		private readonly IAzureStorageRepository _imageRepository;
		private readonly IPersonRepository _personRepository;


		public ImageValidation(IAzureStorageRepository imageRepository,
			IPersonRepository personRepository,
			IHttpContextAccessor contextAccessor) : base(contextAccessor)
		{
			_imageRepository = imageRepository;
			_personRepository = personRepository;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetImage(Guid uid)
		{
			if (!_imageRepository.CheckImageExistance(uid.ToString()).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12, _culture));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetMiniatureImage(Guid imageUid)
		{
			var imageEntity = _personRepository.GetPersonImage(imageUid).Result;
			if (imageEntity == null || !imageEntity.PersonMiniatureImageContentUid.HasValue)
			{
				return (false, ErrorDictionary.GetErrorMessage(12, _culture));
			}
			if (!_imageRepository.CheckImageExistance(imageEntity.PersonMiniatureImageContentUid.ToString()).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12, _culture));
			}
			return (true, string.Empty);
		}
	}
}
