using System;
using Unity.Notifications.Android;
using UnityEngine;

public class Notification : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        CreateNewChannel();
    }

    void Start()
    {
        /*
        //var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");
        int id = 0;
        var notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(id);

        if (notificationStatus == NotificationStatus.Scheduled)
        {
            // Replace the scheduled notification with a new notification.
            //AndroidNotificationCenter.UpdateScheduledNotification(id, newNotification, "channel_id");
            Debug.Log("NotificationStatus.Scheduled");
        }
        else if (notificationStatus == NotificationStatus.Delivered)
        {
            // Remove the previously shown notification from the status bar.
            AndroidNotificationCenter.CancelNotification(id);
            Debug.Log("NotificationStatus.Delivered");
        }
        else if (notificationStatus == NotificationStatus.Unknown)
        {
            Debug.Log("NotificationStatus.Unknown");
            //AndroidNotificationCenter.SendNotification(newNotification, "channel_id");
        }
        
        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
            delegate(AndroidNotificationIntentData data)
            {
                var msg = "Notification received : " + data.Id + "\n";
                msg += "\n Notification received: ";
                msg += "\n .Title: " + data.Notification.Title;
                msg += "\n .Body: " + data.Notification.Text;
                msg += "\n .Channel: " + data.Channel;
                Debug.Log(msg);
            };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
        */
        //var notification2 = new AndroidNotification();
        //notification.IntentData = "{\"title\": \"Notification 1\", \"data\": \"200\"}";
        //AndroidNotificationCenter.SendNotification(notification2, "channel_id");
    }
    
    private void CreateNewChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Senzorové notifikácie",
            EnableVibration = true
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    
    public static void CreateNewNotification(string nodeName, string sensorName, string sensorValue, string sensorUnit )
    {
        string valueAndUnit = sensorValue + sensorUnit;
        var notification = new AndroidNotification();
        notification.Title = "Upozornenie!";
        notification.Text = "Hodnota na "+ sensorName.ToUpper()+"(" + valueAndUnit + ")"+ ", v "+ nodeName.ToUpper()+"!";
        Debug.Log("Hodnota na"+ sensorName.ToUpper()+"(" + valueAndUnit + ")"+ ", v "+ nodeName.ToUpper()+"!");
        notification.FireTime = System.DateTime.Now.AddSeconds(1);
        notification.LargeIcon = "icon_0";
        notification.SmallIcon = "icon_1";
        
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
    
}
