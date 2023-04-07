using TMPro;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] private string sensorName;
    [SerializeField] private string sensorValue;
    [SerializeField] private string sensorUnit;
    [SerializeField] private string physicalQuantity;
    [SerializeField] private string nodeName;

    [SerializeField] private string dataPath;

    
    [SerializeField] private TextMeshProUGUI sensorNameText;
    [SerializeField] private TextMeshProUGUI sensorQuantityText;
    [SerializeField] private TextMeshProUGUI sensorValueText;
    

    public void SetName(string value)
    {
        sensorName = value;
        SetNameText();
    }
    public void SetNameText()
    {
        //sensorNameText.SetText(sensorName); 
    }
    public void SetValue(string value)
    {
        sensorValue = value;
        sensorValueText.SetText(value);
        
        //POZRIE ČI JE HODNOTA NAD HRANICOU
        //CheckValue();
    }
    public void SetUnit(string value)
    {
        sensorUnit = value;
        //sensorValueText.SetText(sensorValueText.text + value); 
        SetValueAndUnitText();
    }
    public void SetValueAndUnitText()
    {
        sensorValueText.SetText(sensorValue + sensorUnit);
    }
    
    public void SetQuantity(string value)
    {
        physicalQuantity = value;
        //sensorQuantityText.SetText(value); 
        SetQuantityText();
    }
    public void SetQuantityText()
    {
        sensorQuantityText.SetText(physicalQuantity);
    }

    public string GetName()
    {
        return sensorName;
    }
    public string GetQuantity()
    {
        return physicalQuantity;
    }
    public string GetValue()
    {
        return sensorValue;
    }
    public string GetUnit()
    {
        return sensorUnit;
    }

    public void CheckValue()
    {
        //Debug.Log(dataPath);
        if (PlayerPrefs.HasKey(dataPath+"/bigger"))
        {
            if (int.Parse(sensorValue)  >  GetThreshold("/bigger"))
            {
                //SPUSTIT NOTIFIKACIU
                Debug.Log("NOTIFIKACIA!"+ dataPath+"/bigger");
                Notification.CreateNewNotification(nodeName, sensorName, sensorValue, sensorUnit,true);
            }
        }
        if (PlayerPrefs.HasKey(dataPath+"/smaller"))
        {
            if (int.Parse(sensorValue)  <  GetThreshold("/smaller"))
            {
                //SPUSTIT NOTIFIKACIU
                Debug.Log("NOTIFIKACIA!"+ dataPath+"/smaller");
                Notification.CreateNewNotification(nodeName, sensorName, sensorValue, sensorUnit,false);
            }
        }
    }

    public void GetDataPath()
    {
        var parent = gameObject.transform.parent.parent.GetComponent<Node>();
        //Debug.Log( parent.GetName());
        nodeName = parent.GetName();
        dataPath = nodeName +"/"+ sensorName;
    }
    
    private int GetThreshold(string compare)
    {
        int threshold = PlayerPrefs.GetInt(dataPath+compare);
        //Debug.Log(dataPath+compare);

        return threshold;
    }
    
    
}
