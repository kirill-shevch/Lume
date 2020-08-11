using BLL.Core.Interfaces;
using Constants;
using DAL.AzureStorage.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace BLL.Core
{
	public class ImageValidation : BaseValidator, IImageValidation
	{
		private readonly IAzureStorageRepository _imageRepository;

		public ImageValidation(IAzureStorageRepository imageRepository,
			IHttpContextAccessor contextAccessor) : base(contextAccessor)
		{
			_imageRepository = imageRepository;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetImage(Guid uid)
		{
			if (!_imageRepository.CheckImageExistance(uid.ToString()).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12, _culture));
			}
			return (true, string.Empty);
		}
	}
}
