using Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Notification.Interfaces
{
	public interface IPushNotificationService
	{
		Task SendPushNotification(string token, MessageTitles messageBodyTitle, Dictionary<FirebaseNotificationKeys, string> data, string title = "Lume", params string[] args);
	}
}
