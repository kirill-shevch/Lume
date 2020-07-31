using BLL.Core.Interfaces;
using BLL.Core.Models.Image;
using Constants;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace BLL.Core
{
	public class ImageValidation : BaseValidator, IImageValidation
	{
		private readonly IPersonRepository _personRepository;
		private readonly IImageRepository _imageRepository;

		public ImageValidation(IPersonRepository personRepository,
			IImageRepository imageRepository,
			IHttpContextAccessor contextAccessor) : base(contextAccessor)
		{
			_personRepository = personRepository;
			_imageRepository = imageRepository;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddPersonImage(AddPersonImageModel model, Guid personUid)
		{
			if (model.Content == null || !model.Content.Any())
			{
				return (false, ErrorDictionary.GetErrorMessage(9, _culture));
			}
			if (!_personRepository.CheckPersonExistence(personUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetPersonImage(Guid uid)
		{
			if (!_imageRepository.CheckPersonImageExistence(uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12, _culture));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetEventImage(Guid uid)
		{
			if (!_imageRepository.CheckEventImageExistence(uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12, _culture));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetChatMessageImage(Guid uid)
		{
			if (!_imageRepository.CheckChatMessageImageExistence(uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12, _culture));
			}

			return (true, string.Empty);
		}
	}
}
