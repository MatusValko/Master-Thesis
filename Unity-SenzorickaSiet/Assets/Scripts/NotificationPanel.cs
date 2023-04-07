using System.Linq;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    //[SerializeField] private FirebaseDatabaseManager firebaseDatabaseManager;
    [SerializeField]  private GameObject sensorSwitchPrefab;
    [SerializeField]  private GameObject sensorNotificationPrefab;
    
    [SerializeField] private GameObject switchedSwitch;

    [SerializeField]  private HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField]  private VerticalLayoutGroup verticalLayoutGroup;

    

    public void ShowNotificationScreen()
    {
        gameObject.SetActive(true);

        foreach (var node in FirebaseDatabaseManager.allNodes)
        {
            GameObject sensorSwitch = Instantiate(sensorSwitchPrefab, new Vector3(0, 0, 0), Quaternion.identity, horizontalLayoutGroup.transform);
            sensorSwitch.GetComponentInChildren<Text>().text = node.GetName();
            Vector2 size3 = horizontalLayoutGroup.GetComponent<RectTransform>().sizeDelta;
            float addToSizeX = sensorSwitchPrefab.GetComponent<RectTransform>().sizeDelta.x + horizontalLayoutGroup.GetComponent<HorizontalLayoutGroup>().spacing;
            horizontalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(size3.x + addToSizeX, size3.y);
            
            string firstNode = FirebaseDatabaseManager.allNodes.First().GetName();

            if (firstNode == node.GetName())
            {
                switchedSwitch = sensorSwitch;
                sensorSwitch.GetComponent<Button_UI>().hoverBehaviour_Color_Exit = new Color32(238, 100, 89, 255);
                sensorSwitch.GetComponentInChildren<Image>().color = new Color32(238, 100, 89, 255);
                GenerateSensorNotifications(node);
                
            }

            sensorSwitch.GetComponent<Button_UI>().ClickFunc = () => {
                if (node.GetAllSensors().Any())
                {
                    switchedSwitch.GetComponentInChildren<Image>().color = Color.white;
                    switchedSwitch.GetComponent<Button_UI>().hoverBehaviour_Color_Exit = Color.white;

                    sensorSwitch.GetComponent<Button_UI>().hoverBehaviour_Color_Exit = new Color32(238, 100, 89, 255);
                    sensorSwitch.GetComponentInChildren<Image>().color = new Color32(238, 100, 89, 255);
                    switchedSwitch = sensorSwitch;
                    DeleteNotificationGameObjects();
                    GenerateSensorNotifications(node);
                }
                else
                {
                    Debug.Log("ERROR");
                }
            };
        }
    }

    private void GenerateSensorNotifications(Node node)
    {
        
        foreach (var sensor in node.GetAllSensors())
        {
            GameObject sensorNotificationGameObject = Instantiate(sensorNotificationPrefab, new Vector3 (0,0,0), Quaternion.identity,verticalLayoutGroup.transform);
            SensorNotification sensorNotification =  sensorNotificationGameObject.GetComponent<SensorNotification>();
            //SET DATA
            sensorNotification.SetNodeName(node.GetName());
            sensorNotification.SetName(sensor.GetName());
            sensorNotification.SetQuantityText(sensor.GetQuantity());
            sensorNotification.SetUnitText(sensor.GetUnit());
            sensorNotification.SetImage();

            Vector2 size = verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta;
            float addToSizeY = sensorNotificationPrefab.GetComponent<RectTransform>().sizeDelta.y + verticalLayoutGroup.spacing;
            //Debug.Log(addToSizeX);
            verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x  ,size.y +addToSizeY);
                    
        }
    }
    
    
    public void CloseNodeDetailScreen()
    {
        DeleteSensors();
        DeleteNotificationGameObjects();
        gameObject.SetActive(false);
    }
    
    private void DeleteSensors()
    {
        foreach (Transform child in horizontalLayoutGroup.transform) {
            Destroy(child.gameObject);
        }
        var sizeDelta = horizontalLayoutGroup.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = 0;
        horizontalLayoutGroup.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }
    private void DeleteNotificationGameObjects() 
    {
        foreach (Transform child in verticalLayoutGroup.transform) {
            Destroy(child.gameObject);
        }
        var sizeDelta = verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = 0;
        verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        
    }


}
