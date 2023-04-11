using System.Collections;
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
    [SerializeField]  private string nodeName;

    [SerializeField]  private GameObject popUpWindow;
    private bool isCoroutineRunning = false;
    
    private void Awake()
    {
        popUpWindow = FindObjectOfType<PopUp>(true).gameObject;
        popUpWindow.SetActive(false);
    }
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
                sensorImage.sprite = FirebaseDatabaseManager.teplotaImage;
                var sizeDelta = sensorImage.gameObject.GetComponent<RectTransform>().sizeDelta;
                sizeDelta.x /= 2;
                sensorImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x,sizeDelta.y);
                var transformPosition = sensorImage.gameObject.transform.localPosition;
                transformPosition.x = -390;
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
                Debug.Log("Nenaslo");
                break;
        }
    }

    public void SaveNotificationData()
    {
        if (!string.IsNullOrEmpty(valueText1.text))
        {
            var value1 = int.Parse(valueText1.text);
            string dataPath = nodeName +"/"+ sensorName+"/bigger" ;
            PlayerPrefs.SetInt(dataPath , value1);
        }
        else
        {
            PlayerPrefs.DeleteKey(nodeName +"/"+ sensorName+"/bigger");
        }
        if (!string.IsNullOrEmpty(valueText2.text))
        {
            var value2 = int.Parse(valueText2.text);
            string dataPath = nodeName +"/"+ sensorName+"/smaller" ;
            PlayerPrefs.SetInt(dataPath , value2);
        }
        else
        {
            PlayerPrefs.DeleteKey(nodeName +"/"+ sensorName+"/smaller");
        }
        
        popUpWindow.SetActive(true);
        var transformPosition = popUpWindow.transform.position;
        transformPosition.y= 0;
        popUpWindow.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);

        if (!isCoroutineRunning)
        {
            StartCoroutine(ActivateObjectWithDelay());
        }}

    IEnumerator ActivateObjectWithDelay() {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(3);
        popUpWindow.SetActive(false);
        isCoroutineRunning = false;
    }
    
    public void AssignNotificationData()
    {
        if (PlayerPrefs.HasKey(nodeName + "/" + sensorName + "/bigger"))
        {
            GetComponentsInChildren<InputField>()[0].text = PlayerPrefs.GetInt(nodeName + "/" + sensorName + "/bigger").ToString();
        }
        if (PlayerPrefs.HasKey(nodeName + "/" + sensorName + "/smaller"))
        {
            GetComponentsInChildren<InputField>()[1].text = PlayerPrefs.GetInt(nodeName + "/" + sensorName + "/smaller").ToString();
        }
    }
}
