using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FirebaseDatabaseManager : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject sensorPrefab;
    [SerializeField] private GameObject nodeGrid;
    
    [SerializeField] private NotificationPanel notificationPanel;
    [SerializeField] private int logsNumber = 30;

    private DataSnapshot snapshot;
    public DataSnapshot logSnapshot;
    private bool first = false;
    
    public static List<Node>  allNodes = new List<Node>();

    


    private async void Awake()
    {
        //TODO: VYMAZAT POTOM
        PlayerPrefs.DeleteAll();
        
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        
        /*
        Debug.Log("CHECKING DEPENDENCIES");
        await FirebaseApp.CheckAndFixDependenciesAsync();
        Debug.Log("GETTING DATA");
        await GetData();
        Debug.Log("SHOW DATA");
        ShowNodesDetail();
        */
        
        GetAndShowData();
        await GetValueChanged();
    }
    
    private void AddData()
    {
        DatabaseReference reference;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
        float temperature = Random.Range(1.0f, 40.0f);
        string currDate = DateTime.Now.ToString("dd/MM/yyyy/");
        string currTime = DateTime.Now.ToString("HH:mm:ss");
        string time = currDate + currTime;
        
        Debug.Log(time);
        
        
        reference.Child("Data").Child("Uzol_1").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_1").Child("Teplomer").Child("Velicina").SetValueAsync("Teplota");
        reference.Child("Data").Child("Uzol_1").Child("Teplomer").Child("Hodnota").SetValueAsync(temperature);
        reference.Child("Data").Child("Uzol_1").Child("Teplomer").Child("Jednotka").SetValueAsync("°C");
            
        reference.Child("Data").Child("Uzol_1").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_1").Child("Vlhkomer").Child("Velicina").SetValueAsync("Vlhkosť");
        reference.Child("Data").Child("Uzol_1").Child("Vlhkomer").Child("Hodnota").SetValueAsync(temperature);
        reference.Child("Data").Child("Uzol_1").Child("Vlhkomer").Child("Jednotka").SetValueAsync("%");
        
        reference.Child("Data").Child("Uzol_1").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_1").Child("Oxid uhličitý").Child("Velicina").SetValueAsync("Co2");
        reference.Child("Data").Child("Uzol_1").Child("Oxid uhličitý").Child("Hodnota").SetValueAsync(temperature);
        reference.Child("Data").Child("Uzol_1").Child("Oxid uhličitý").Child("Jednotka").SetValueAsync("%");

        reference.Child("Data").Child("Uzol_1").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_1").Child("Svetlomer").Child("Velicina").SetValueAsync("Svietivosť");
        reference.Child("Data").Child("Uzol_1").Child("Svetlomer").Child("Hodnota").SetValueAsync(temperature);
        reference.Child("Data").Child("Uzol_1").Child("Svetlomer").Child("Jednotka").SetValueAsync("Lux");
        
        reference.Child("Data").Child("Uzol_1").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_1").Child("Hlukomer").Child("Velicina").SetValueAsync("Hlučnosť");
        reference.Child("Data").Child("Uzol_1").Child("Hlukomer").Child("Hodnota").SetValueAsync(temperature);
        reference.Child("Data").Child("Uzol_1").Child("Hlukomer").Child("Jednotka").SetValueAsync("Db");
        
        reference.Child("Data").Child("Uzol_2").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_2").Child("Teplomer").Child("Velicina").SetValueAsync("Teplota");
        reference.Child("Data").Child("Uzol_2").Child("Teplomer").Child("Hodnota").SetValueAsync(temperature/2);
        reference.Child("Data").Child("Uzol_2").Child("Teplomer").Child("Jednotka").SetValueAsync("°C");
        
        reference.Child("Data").Child("Uzol_3").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_3").Child("Teplomer").Child("Velicina").SetValueAsync("Teplota");
        reference.Child("Data").Child("Uzol_3").Child("Teplomer").Child("Hodnota").SetValueAsync(temperature/4);
        reference.Child("Data").Child("Uzol_3").Child("Teplomer").Child("Jednotka").SetValueAsync("°C");
        
        reference.Child("Data").Child("Uzol_4").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_4").Child("Teplomer").Child("Velicina").SetValueAsync("Teplota");
        reference.Child("Data").Child("Uzol_4").Child("Teplomer").Child("Hodnota").SetValueAsync(temperature/8);
        reference.Child("Data").Child("Uzol_4").Child("Teplomer").Child("Jednotka").SetValueAsync("°C");
        
        reference.Child("Data").Child("Uzol_5").Child("Cas").SetValueAsync(time);
        reference.Child("Data").Child("Uzol_5").Child("Teplomer").Child("Velicina").SetValueAsync("Teplota");
        reference.Child("Data").Child("Uzol_5").Child("Teplomer").Child("Hodnota").SetValueAsync(temperature*2);
        reference.Child("Data").Child("Uzol_5").Child("Teplomer").Child("Jednotka").SetValueAsync("°C");
    }


    private async void GetAndShowData()
    {
        Debug.Log("KONTROLA ZÁVISLOSTÍ");
        await FirebaseApp.CheckAndFixDependenciesAsync();
        
        Debug.Log("ZÍSKAVANIE ÚDAJOV");
        await GetData();
        
        Debug.Log("ZOBRAZENIE ÚDAJOV");
        ShowNodesDetail();
    }

    
    private Task GetValueChanged()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Data")
            .ValueChanged += HandleValueChanged;
        return Task.CompletedTask;
    }
    
    private void HandleValueChanged(object sender, ValueChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if (first)
        {
            Debug.Log("HODNOTY SA ZMENILI");
            foreach (var node in allNodes)
            {
                if (node)
                {
                    Destroy(node.gameObject);
                }
            }
            allNodes.Clear();
            nodeGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(900,0);
            Invoke( nameof(GetAndShowData),1);
        }
        else
        {
            first = true;
        }
    }
    

    
    
    
    private async Task GetData()
    { 
        await FirebaseDatabase.DefaultInstance.
            GetReference("Data").GetValueAsync().
            ContinueWithOnMainThread(task => {
                if (task.IsFaulted|| task.IsCanceled) {
                    Debug.Log("CHYBA! :" + task);
                }
                else if (task.IsCompleted ) {
                    snapshot = task.Result;
                }
            });
    }

    public async Task GetLogs(string node)
    {
        if (string.IsNullOrEmpty(node) || node == "Chyba")
        {
            Debug.Log("Chyba");
            return;
        }
        
        await FirebaseDatabase.DefaultInstance.
            GetReference("Logs/"+ node ).LimitToFirst(logsNumber).GetValueAsync()
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted|| task.IsCanceled) {
                    Debug.Log("CHYBA! :" + task);
                }
                else if (task.IsCompleted )
                {
                    logSnapshot = task.Result;
                }
            });
    }
    

    public void ShowNodes()
    {
        foreach (var node in allNodes)
        {
            Debug.Log(node.GetName());
            
            foreach (var sensor in node.GetAllSensors())
            {
                Debug.Log(sensor.GetName());
            }
            Debug.Log("-------------------------");
        }
    }

    private void ShowNodesDetail()
    {
        foreach (var variable in snapshot.Children)
        {
            //ZVACSI GRID O JEDNO OKNO
            Vector2 size = nodeGrid.GetComponent<RectTransform>().sizeDelta;
            nodeGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,size.y + nodeGrid.GetComponent<GridLayoutGroup>().cellSize.y + nodeGrid.GetComponent<GridLayoutGroup>().spacing.y);
            //VYTVOR OKNO
            GameObject nodeGameObject = Instantiate(nodePrefab, new Vector3 (0,0,0), Quaternion.identity,nodeGrid.transform) ;
            Node actualNode = nodeGameObject.GetComponent<Node>();
            actualNode.SetName(variable.Key);
            allNodes.Add(actualNode);
            //PRIDAJ DO OKNA NAMERANE VELICINY
            GameObject sensorGrid = nodeGameObject.GetComponentInChildren<GridLayoutGroup>().gameObject;
            //MAX 2 OKNA
            int okno = 0;
            foreach (var sensor in variable.Children)
            {
                if (sensor.Key != "Timestamp" )
                {
                    GameObject sensorGameObject = Instantiate(sensorPrefab, new Vector3 (0,0,0), Quaternion.identity,sensorGrid.transform);
                    Sensor actualSensor = sensorGameObject.GetComponent<Sensor>();
                    actualNode.AddNewSensor(actualSensor);
                    actualSensor.SetName(sensor.Key);
                    actualSensor.GetDataPath();
                    foreach (var sensorDATA in sensor.Children)
                    {
                        string data = sensorDATA.Key;
                        //Debug.Log(data);
                        switch (data)
                        {
                            // 0 a 1 JE HODNOTA A JEDNOTKA, 3 JE MENO VELICINY
                            case "Hodnota": 
                                actualSensor.SetValue(sensorDATA.GetValue(true).ToString());
                                break;
                            case "Jednotka":
                                actualSensor.SetUnit(sensorDATA.GetValue(true).ToString());
                                break;
                            case "Velicina":
                                actualSensor.SetQuantity(sensorDATA.GetValue(true).ToString());
                                break;
                            default: 
                                Debug.Log("Nenaslo");
                                break;
                        }
                    }
                    actualSensor.CheckValue();
                    
                    //TEPLOMER A VLHKOMER SU VZDY UKAZANE AK SA NACHADZAJU
                    if (actualSensor.GetName() == "Teplomer" || actualSensor.GetName() == "Vlhkomer")
                    {
                        if (okno >= 2)
                        {
                            actualSensor.gameObject.SetActive(false);
                        }
                        else
                        {
                            okno++;
                        }
                    }
                    else
                    {
                        actualSensor.gameObject.SetActive(false);
                    }
                }
                
            }
            if (okno<2)
            {
                if (actualNode.GetAllSensors().First())
                {
                    actualNode.GetAllSensors().First().gameObject.SetActive(true);
                    okno++;
                    if (okno<2)
                    {
                        if (actualNode.GetAllSensors().Last())
                        {
                            actualNode.GetAllSensors().Last().gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        
        ChangeOrderInSensorList();
        //ShowNodes();
    }

    private void ChangeOrderInSensorList()
    {
        foreach (var node in allNodes)
        {
            //Debug.Log(node.GetName());
            for (int i = 0; i < node.GetAllSensors().Count; i++)
            {
                if (node.GetAllSensors()[i].GetName() == "Teplomer")
                {
                    Sensor tmp;
                    tmp = node.GetAllSensors()[0];
                    node.GetAllSensors()[0] = node.GetAllSensors()[i];
                    node.GetAllSensors()[i] = tmp;
                }
                else if (node.GetAllSensors()[i].GetName() == "Vlhkomer")
                {
                    Sensor tmp;
                    tmp = node.GetAllSensors()[1];
                    node.GetAllSensors()[1] = node.GetAllSensors()[i];
                    node.GetAllSensors()[i] = tmp;

                }
            }
        }
    }

    public void ShowNotificationScreen()
    {
        notificationPanel = Resources.FindObjectsOfTypeAll<NotificationPanel>()[0];
        if (allNodes != null)
        {
            notificationPanel.ShowNotificationScreen();

        }
        else
        {
            Debug.Log("Not ready!");
        }
        
    }
    
}