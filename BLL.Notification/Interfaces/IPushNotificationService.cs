using Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Notification.Interfaces
{
	public interface IPushNotificationService
	{
		Task SendPushNotification(string token, MessageTitles messageTitle, Dictionary<FirebaseNotificationKeys, string> data, params string[] args);
	}
}
