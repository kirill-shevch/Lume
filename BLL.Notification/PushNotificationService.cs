﻿using BLL.Notification.Interfaces;
using Constants;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace BLL.Notification
{
	public class PushNotificationService : IPushNotificationService
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _contextAccessor;
		public PushNotificationService(IConfiguration configuration,
			IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
			_configuration = configuration;
		}

		public async Task SendPushNotification(string token, MessageTitles messageBodyTitle, Dictionary<FirebaseNotificationKeys, string> data, string title, params string[] args)
		{
			var key = _configuration.GetValue<string>(ConfigurationKeys.FirebaseKey);
			if (!string.IsNullOrEmpty(key))
			{
				if (FirebaseApp.DefaultInstance == null)
				{
					FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromJson(key) });
				}
				var httpContext = _contextAccessor?.HttpContext;
				var culture = httpContext == null ? CultureParser.GetDefaultCulture() : CultureParser.GetCultureFromHttpContext(httpContext);
				var message = new Message
				{
					Notification = new FirebaseAdmin.Messaging.Notification
					{
						Title = title,
						Body = string.Format(Messages.GetMessage(messageBodyTitle, culture), args)
					},
					Token = token
				};
				if (data != null && data.Any())
				{
					message.Data = data.ToDictionary(x => x.Key.ToString(), x => x.Value);
				}
				try
				{
					await FirebaseMessaging.DefaultInstance.SendAsync(message);
				}
				catch
				{
				}
			}
		}
	}
}