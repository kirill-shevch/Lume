using BLL.Core.Interfaces;
using BLL.Core.Models;
using BLL.Core.Models.Image;
using Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/image")]
	[ApiController]
	public class ImageController : ControllerBase
	{
		private readonly IImageValidation _imageValidation;
		private readonly IImageLogic _imageLogic;

		public ImageController(IImageValidation imageValidation,
			IImageLogic imageLogic)
		{
			_imageValidation = imageValidation;
			_imageLogic = imageLogic;
		}

		[HttpPost]
		[Route("add-person-image")]
		public async Task<ActionResult<AddImageResponse>> AddPersonImage(AddPersonImageModel addImageModel)
		{
			var personUid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _imageValidation.ValidateAddPersonImage(addImageModel, personUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var uid = await _imageLogic.SavePersonImage(addImageModel, personUid);
			return new AddImageResponse { ImageUid = uid };
		}

		[HttpGet]
		[Route("get-person-image")]
		public async Task<ActionResult> GetPersonImage(Guid imageUid)
		{
			var validationResult = _imageValidation.ValidateGetPersonImage(imageUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var content = await _imageLogic.GetPersonImage(imageUid);
			return File(content, "image/jpeg");
		}

		[HttpPost]
		[Route("add-event-image")]
		public async Task<ActionResult<AddImageResponse>> AddEventImage(AddImageModel addImageModel)
		{
			var validationResult = _imageValidation.ValidateAddEventImage(addImageModel);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var uid = await _imageLogic.SaveEventImage(addImageModel);
			return new AddImageResponse { ImageUid = uid };
		}

		[HttpGet]
		[Route("get-event-image")]
		public async Task<ActionResult> GetEventImage(Guid imageUid)
		{
			var validationResult = _imageValidation.ValidateGetEventImage(imageUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var content = await _imageLogic.GetEventImage(imageUid);
			return File(content, "image/jpeg");
		}
	}
}
