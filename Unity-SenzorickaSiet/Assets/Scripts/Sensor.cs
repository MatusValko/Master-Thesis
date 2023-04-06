using System;
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


    private void Awake()
    {
        

    }

    private void Start()
    {
        //SetThreshold();
    }

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
        SetThreshold();
        //Debug.Log( GetThreshold());
        //Debug.Log( int.Parse(sensorValue));

        
        if (int.Parse(sensorValue)  >  GetThreshold())
        {
            //SPUSTIT NOTIFIKACIU
            //Debug.Log("NOTIFIKACIA!"+ dataPath);
            //Debug.Log(sensorUnit);

            Notification.CreateNewNotification(nodeName, sensorName, sensorValue, sensorUnit);
        }
    }

    public void GetDataPath()
    {
        var parent = gameObject.transform.parent.parent.GetComponent<Node>();
        //Debug.Log( parent.GetName());
        nodeName = parent.GetName();
        dataPath = nodeName +"/"+ sensorName;
    }
    
    private int GetThreshold()
    {
        int threshold = PlayerPrefs.GetInt(dataPath);
        return threshold;
    }
    
    public void SetThreshold(int threshold = 100)
    {
        /*
        if (!PlayerPrefs.HasKey(dataPath))
        {
            PlayerPrefs.SetInt(dataPath , threshold);
        }*/
        PlayerPrefs.SetInt(dataPath , threshold);
    }
    
    
}
