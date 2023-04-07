using UnityEngine;
using UnityEngine.UI;

public class SensorNotification : MonoBehaviour
{
    [SerializeField] private Text sensorQuantity;
    [SerializeField] private string sensorName;
    [SerializeField] private Text sensorUnit;
    [SerializeField] private Text sensorUnit2;
    
    [SerializeField] private Text valueText1;
    [SerializeField] private Text valueText2;

    [SerializeField]  private Image sensorImage;
    
    [SerializeField]  private Sprite teplotaImage;
    [SerializeField]  private Sprite vlhkostImage;

    [SerializeField]  private Sprite oxidUholnatyImage;
    [SerializeField]  private Sprite osvetlenieImage;
    [SerializeField]  private Sprite hlukImage;
    [SerializeField]  private Sprite pohybImage;

    [SerializeField]  private string nodeName;

    
    public void SetNodeName(string value)
    {
        nodeName = value; 
    }
    public void SetName(string value)
    {
        sensorName = value; 
    }
    public void SetQuantityText(string value)
    {
        sensorQuantity.text = value; 
    }
    
    public void SetUnitText(string value)
    {
        sensorUnit.text = value;
        sensorUnit2.text = value; 
    }
    
    public void SetImage()
    {
        switch (sensorQuantity.text)
        { 
            case "Teplota":
                sensorImage.sprite = teplotaImage;
                var sizeDelta = sensorImage.gameObject.GetComponent<RectTransform>().sizeDelta;
                sizeDelta.x /= 2;
                sensorImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x,sizeDelta.y);
                var transformPosition = sensorImage.gameObject.transform.localPosition;
                transformPosition.x = -390;
                sensorImage.gameObject.transform.localPosition = new Vector3(transformPosition.x,transformPosition.y,transformPosition.z) ;
                break; 
            case "Vlhkosť":
               sensorImage.sprite = vlhkostImage;
               break;
            case "Oxid uhoľnatý":
                sensorImage.sprite = oxidUholnatyImage;
                break;
            case "Osvetlenie":
                sensorImage.sprite = osvetlenieImage;
                break;
            case "Hluk":
                sensorImage.sprite = hlukImage;
                break;
            case "Pohyb":
                sensorImage.sprite = pohybImage;
                break;

            default: 
                //Debug.Log("Nenaslo");
                break;

        }
    }

    public void SaveNotificationData()
    {
        int value1;
        int value2;
        if (string.IsNullOrEmpty(valueText1.text))
        {
            value1 = 0;
        }
        else
        {
            value1 = int.Parse(valueText1.text);
        }
        if (string.IsNullOrEmpty(valueText2.text))
        {
            value2 = 0;
        }
        else
        {
            value2 = int.Parse(valueText2.text);
        }
        
        //Debug.Log(value1+" " + value2);
        
        if (value1 != 0)
        {
            string dataPath = nodeName +"/"+ sensorName+"/bigger" ;
            PlayerPrefs.SetInt(dataPath , value1);
            //Debug.Log("Set INT"+dataPath+""+value1);
        }
        if (value2 != 0)
        {
            string dataPath = nodeName +"/"+ sensorName+"/smaller" ;
            PlayerPrefs.SetInt(dataPath , value2);
            //Debug.Log("Set INT"+dataPath+""+value2);

        }
    }
    
    
}
