using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace Notification
{
    public class NotificationManager : MonoBehaviour
    {
        // Define a unique identifier for the notification channel
        private static readonly string CHANNEL_ID = "channel_id";

        private void Awake()
        {
            InitializeAndroidNotificationChannel();
        }

        // Create and register an Android notification channel
        private void InitializeAndroidNotificationChannel()
        {
            AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel()
            {
                Id = CHANNEL_ID,
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications"
            };

            AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // Send a notification when the application is paused
            if (pauseStatus)
            {
                SendPauseNotification();
            }
        }

        private void SendPauseNotification()
        {
            AndroidNotification androidNotification = new AndroidNotification()
            {
                Title = "Hello!",
                Text = "Come back and play Fruit Slicer!",
                FireTime = DateTime.Now.AddSeconds(20.0)
            };

            // Cancel any existing pause notifications and send a new one
            AndroidNotificationCenter.CancelNotification((int)NotificationID.PauseNotification);
            AndroidNotificationCenter.SendNotificationWithExplicitID(androidNotification, CHANNEL_ID, (int)NotificationID.PauseNotification);
        }

        // Send a notification when the settings button is tapped
        public void SendSettingsButtonNotification()
        {
            AndroidNotification androidNotification = new AndroidNotification()
            {
                Title = "Settings button tapped",
                Text = "Here is my notification for you to KEEP PLAYING :)",
                FireTime = DateTime.Now.AddSeconds(2.0)
            };

            // Cancel any existing settings button notifications and send a new one
            AndroidNotificationCenter.CancelNotification((int)NotificationID.SettingsButtonNotification);
            AndroidNotificationCenter.SendNotification(androidNotification, CHANNEL_ID);
        }
    }
}
