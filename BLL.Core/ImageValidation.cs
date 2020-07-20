using BLL.Core.Interfaces;
using BLL.Core.Models.Image;
using Constants;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace BLL.Core
{
	public class ImageValidation : IImageValidation
	{
		private readonly IPersonRepository _personRepository;
		private readonly IEventRepository _eventRepository;
		private readonly IChatRepository _chatRepository;
		private readonly IImageRepository _imageRepository;

		public ImageValidation(IPersonRepository personRepository,
			IEventRepository eventRepository,
			IChatRepository chatRepository,
			IImageRepository imageRepository)
		{
			_personRepository = personRepository;
			_eventRepository = eventRepository;
			_chatRepository = chatRepository;
			_imageRepository = imageRepository;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddPersonImage(AddPersonImageModel model, Guid personUid)
		{
			if (model.Content == null || !model.Content.Any())
			{
				return (false, ErrorDictionary.GetErrorMessage(9));
			}
			if (!_personRepository.CheckPersonExistence(personUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetPersonImage(Guid uid)
		{
			if (!_imageRepository.CheckPersonImageExistence(uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddEventImage(AddImageModel model)
		{
			if (model.Content == null || !model.Content.Any())
			{
				return (false, ErrorDictionary.GetErrorMessage(9));
			}
			if (!_eventRepository.CheckEventExistence(model.Uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetEventImage(Guid uid)
		{
			if (!_imageRepository.CheckEventImageExistence(uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddChatMessageImage(AddImageModel model)
		{
			if (model.Content == null || !model.Content.Any())
			{
				return (false, ErrorDictionary.GetErrorMessage(9));
			}
			if (!_chatRepository.CheckChatMessageExistence(model.Uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(11));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetChatMessageImage(Guid uid)
		{
			if (!_imageRepository.CheckChatMessageImageExistence(uid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(12));
			}

			return (true, string.Empty);
		}
	}
}
