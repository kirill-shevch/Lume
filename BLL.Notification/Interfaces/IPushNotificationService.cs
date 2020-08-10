using Constants;
using System.Threading.Tasks;

namespace BLL.Notification.Interfaces
{
	public interface IPushNotificationService
	{
		Task SendPushNotification(string token, MessageTitles messageTitle, params string[] args);
	}
}
