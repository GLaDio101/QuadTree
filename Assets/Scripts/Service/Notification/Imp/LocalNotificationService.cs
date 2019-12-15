using System;
using UnityEngine;

namespace Service.Notification.Imp
{
    public class LocalNotificationService : INotificationService
    {
		#if UNITY_IOS
		[PostConstruct]
		public void OnPostConstruct()
		{ 
			UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
			
		
		}
		#endif
        public void Cancel(int id)
        {
#if UNITY_ANDROID
            LocalNotification.CancelNotification(id);
#elif UNITY_IOS
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
#endif
        }

        public void Schedule(int id, int afterSeconds, string message)
        {
#if UNITY_ANDROID
            LocalNotification.SendNotification(id, afterSeconds, Application.productName, message, new Color32(0xff, 0xff, 0xff, 255), true, true, true, "app_icon");
#elif UNITY_IOS
			var notif = new UnityEngine.iOS.LocalNotification();
            notif.fireDate = DateTime.Now.AddSeconds(afterSeconds);
            notif.alertBody = message;
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
#endif
        }
    }
}