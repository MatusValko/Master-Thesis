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
    
    public static void CreateNewNotification(string nodeName, string sensorName, string sensorValue, string sensorUnit , bool compare)
    {
        string compared;
        //1 je vacsia, 0 je mensia
        compared = compare ? "väčšia" : "menšia";
        
        string valueAndUnit = sensorValue + sensorUnit;
        var notification = new AndroidNotification();
        notification.Title = "Upozornenie v "+nodeName.ToUpper();
        notification.Text = "Hodnota je "+compared+" na "+ sensorName.ToUpper()+"(" + valueAndUnit + ")!";
        Debug.Log("Hodnota je "+ compared+" na "+ sensorName.ToUpper()+"(" + valueAndUnit + ")!");
        notification.FireTime = System.DateTime.Now.AddSeconds(1);
        notification.LargeIcon = "icon_0";
        notification.SmallIcon = "icon_1";
        
        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
    
}
