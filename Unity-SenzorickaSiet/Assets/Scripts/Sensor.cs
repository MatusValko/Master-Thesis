using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sensor : MonoBehaviour
{
    [SerializeField] private string sensorName;
    [SerializeField] private string sensorValue;
    [SerializeField] private string sensorUnit;
    [SerializeField] private string physicalQuantity;
    [SerializeField] private string nodeName;

    [SerializeField] private string dataPath;

    [SerializeField]  private Image sensorImage;

    [SerializeField] private TextMeshProUGUI sensorNameText;
    [SerializeField] private Text sensorQuantityText;
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
    }
    public void SetQuantityText()
    {
        sensorQuantityText.text=physicalQuantity;
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
    
    public void SetImage( )
    {
        //Sprite sensorImage;
        switch (physicalQuantity)
        { 
            case "Teplota":
                sensorImage.sprite = FirebaseDatabaseManager.teplotaImage;
                
                var sizeDelta = sensorImage.gameObject.GetComponent<RectTransform>().sizeDelta;
                sizeDelta.x /= 2;
                sensorImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x,sizeDelta.y);
                var transformPosition = sensorImage.gameObject.transform.localPosition;
                transformPosition.x += 15;
                sensorImage.gameObject.transform.localPosition = new Vector3(transformPosition.x,transformPosition.y,transformPosition.z) ;
                break; 
            case "Vlhkosť":
                sensorImage.sprite = FirebaseDatabaseManager.vlhkostImage;
                break;
            case "Oxid uhoľnatý":
                sensorImage.sprite = FirebaseDatabaseManager.oxidUholnatyImage;
                break;
            case "Intenzita osvetlenia":
                sensorImage.sprite = FirebaseDatabaseManager.osvetlenieImage;
                break;
            case "Intenzita zvuku":
                sensorImage.sprite = FirebaseDatabaseManager.hlukImage;
                break;
            case "Pohyb":
                sensorImage.sprite = FirebaseDatabaseManager.pohybImage;
                break;
            case "Oxid uhličitý":
                sensorImage.sprite = FirebaseDatabaseManager.oxidUhličitýImage;
                break;
            case "Dym":
                sensorImage.sprite = FirebaseDatabaseManager.smokeImage;
                break;
            default: 
                //Debug.Log("Nenaslo");
                break;

        }
    }
    
}
