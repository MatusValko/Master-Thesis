using System;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

public class DetailedNode : MonoBehaviour
{
    [SerializeField] private GameObject sensorPrefab;
    [SerializeField] private GameObject graphPrefab;
    [SerializeField]  private GameObject sensorSwitchPrefab;

    [SerializeField] private GameObject sensorGrid;
    [SerializeField] private GameObject verticalLayoutGroup;
    [SerializeField] private Window_Graph windowGraph;
    
    [SerializeField] private Text nodeNameText;
    [SerializeField] private FirebaseDatabaseManager firebaseDatabaseManager;
    [SerializeField] private GameObject switchedSwitch;

    [SerializeField] private List<int> valueList = new List<int>();
    [SerializeField] private List<string> timeList = new List<string>();

    [SerializeField] private string unit;
    [SerializeField] private string quantity;
    
    

    
    public void ShowNodeDetailScreen(string nazov = "Chyba")
    {
        gameObject.SetActive(true);
        
        if (string.IsNullOrEmpty(nazov) || nazov == "Chyba")
        {
            nodeNameText.text ="Chyba";
            return;
        }

        foreach (var node in FirebaseDatabaseManager.allNodes)
        {
            if (node.GetName() == nazov)
            {
                //GENERATE GRAPH
                
                GameObject graphGameObject = Instantiate(graphPrefab, new Vector3 (0,0,0), Quaternion.identity,verticalLayoutGroup.transform);
                windowGraph = graphGameObject.GetComponent<Window_Graph>();
                string firstSensor = node.GetAllSensors().First().GetName();
                InitGraph(node.GetName(),firstSensor,windowGraph);
                
                nodeNameText.text = node.GetName();
                int i = 0;
                foreach (var sensor in node.GetAllSensors())
                {
                    GameObject sensorGameObject = Instantiate(sensorPrefab, new Vector3 (0,0,0), Quaternion.identity,sensorGrid.transform);
                    Sensor actualSensor =  sensorGameObject.GetComponent<Sensor>();
                    //SET DATA
                    actualSensor.SetName(sensor.GetName());
                    actualSensor.SetQuantity(sensor.GetQuantity());
                    actualSensor.SetValue(sensor.GetValue());
                    actualSensor.SetUnit(sensor.GetUnit());
                    //SET TEXT
                    actualSensor.SetQuantityText();
                    actualSensor.SetValueAndUnitText();
                    
                    GameObject sensorSwitch = Instantiate(sensorSwitchPrefab, new Vector3 (0,0,0), Quaternion.identity,windowGraph.HorizontalLayoutGroup.transform);
                    sensorSwitch.GetComponentInChildren<Text>().text = sensor.GetName();
                    Vector2 size3 = windowGraph.HorizontalLayoutGroup.GetComponent<RectTransform>().sizeDelta;
                    float addToSizeX = sensorSwitchPrefab.GetComponent<RectTransform>().sizeDelta.x + windowGraph.HorizontalLayoutGroup.GetComponent<HorizontalLayoutGroup>().spacing;
                    //Debug.Log(addToSizeX);
                    windowGraph.HorizontalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(size3.x + addToSizeX ,size3.y);
                    if (firstSensor == sensor.GetName())
                    {
                        switchedSwitch = sensorSwitch;
                        sensorSwitch.GetComponent<Button_UI>().hoverBehaviour_Color_Exit = new Color32(238,100,89,255);
                        sensorSwitch.GetComponentInChildren<Image>().color = new Color32(238,100,89,255);
                    }
                    sensorSwitch.GetComponent<Button_UI>().ClickFunc = () => {
                        GetLog(sensor.GetName());
                        if (valueList.Any())
                        {
                            windowGraph.ShowGraph(valueList,windowGraph.graphVisual,-1,(int _i) => timeList[_i],(float _f) =>  Mathf.RoundToInt(_f)+ unit);
                            switchedSwitch.GetComponentInChildren<Image>().color = Color.white;
                            switchedSwitch.GetComponent<Button_UI>().hoverBehaviour_Color_Exit = Color.white;;
                            sensorSwitch.GetComponent<Button_UI>().hoverBehaviour_Color_Exit = new Color32(238,100,89,255);
                            sensorSwitch.GetComponentInChildren<Image>().color = new Color32(238,100,89,255);
                            switchedSwitch = sensorSwitch;
                        }
                        else
                        {
                            Debug.Log("ERROR");
                        }

                    };
                    
                    
                    if (i % 2 == 0)
                    {
                        Vector2 size = sensorGrid.GetComponent<RectTransform>().sizeDelta;
                        sensorGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,size.y + sensorGrid.GetComponent<GridLayoutGroup>().cellSize.y + sensorGrid.GetComponent<GridLayoutGroup>().spacing.y);
                    }
                    i++;
                }
                
                Vector2 size2 = sensorGrid.GetComponent<RectTransform>().sizeDelta;
                verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(size2.x,size2.y + 800);
                
                break;
            }
        }
    }

    private async void InitGraph(string nodeName,string sensorName, Window_Graph windowGraph)
    {
        if (firebaseDatabaseManager.logSnapshot == null || firebaseDatabaseManager.logSnapshot.Key != nodeName)
        {
            Debug.Log("ZÍSKAVANIE ZÁZNAMOV");
            await firebaseDatabaseManager.GetLogs(nodeName);
        }
        Debug.Log("UKÁZANIE ZÁZNAMU");
        GetLog(sensorName);
        
        windowGraph.ShowGraph(valueList,windowGraph.barChartVisual,-1,(int _i) => timeList[_i],(float _f) =>  Mathf.RoundToInt(_f)+ unit);
    }
    
    
    
    public void GetLog(string sensorName)
    {
        valueList.Clear();
        timeList.Clear();
        foreach (var unique in firebaseDatabaseManager.logSnapshot.Children)
        {
            //Debug.Log(firebaseDatabaseManager.logSnapshot.Key);
            foreach (var sensor in unique.Children)
            {
                if (sensor.Key == "Timestamp" )//&& okno <= 3
                {
                    string time;
                    //Debug.Log($"Timestamp: " + sensor.Value );
                    //Debug.Log(UnixTimeStampToDateTime(double.Parse(sensor.GetValue(true).ToString())));
                    //Debug.Log(DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(sensor.GetValue(true).ToString())).LocalDateTime.Hour);
                    time =  DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(sensor.GetValue(true).ToString())).LocalDateTime.Hour.ToString();
                    //time += ":" + DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(sensor.GetValue(true).ToString())).LocalDateTime.Minute;
                    timeList.Add(time);
                }
                else if (sensor.Key == sensorName )
                {
                    foreach (var sensorDATA in sensor.Children)
                    {
                        string data = sensorDATA.Key;
                        switch (data)
                        {
                            // 0 a 1 JE HODNOTA A JEDNOTKA, 3 JE MENO VELICINY
                            case "Hodnota":
                                int number =int.Parse(sensorDATA.GetValue(true).ToString());
                                valueList.Add(number);
                                //Debug.Log("NUMBER= "+number);
                                break;
                            case "Jednotka":
                                string unit = sensorDATA.GetValue(true).ToString();
                                this.unit = unit;
                                break;
                            case "Velicina":
                                string quantity = sensorDATA.GetValue(true).ToString();
                                this.quantity = quantity;
                                break;
                            default: 
                                Debug.Log("Nenaslo");
                                break;
                        }
                    }
                }

            }
        }
    }
    
    public void CloseNodeDetailScreen()
    {
        DeleteSensors();
        DeleteGraph();
        gameObject.SetActive(false);
    }
    
    private void DeleteSensors()
    {
        foreach (Transform child in sensorGrid.transform) {
            Destroy(child.gameObject);
        }
        var sizeDelta = sensorGrid.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = 0;
        sensorGrid.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }
    private void DeleteGraph()
    {
        Destroy(windowGraph.gameObject);

    }

}
