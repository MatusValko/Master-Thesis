using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;


public class FirebaseDatabaseManager : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject sensorPrefab;
    [SerializeField] private GameObject nodeGrid;
    
    [SerializeField] private NotificationPanel notificationPanel;
    [SerializeField] private int logsNumber = 30;
    
    
    [SerializeField] private Sprite teplotaImageIcon;
    [SerializeField] private Sprite vlhkostImageIcon;
    [SerializeField] private Sprite oxidUholnatyImageIcon;
    [SerializeField] private Sprite oxidUhličitýImageIcon;
    [SerializeField] private Sprite osvetlenieImageIcon;
    [SerializeField] private Sprite hlukImageIcon;
    [SerializeField] private Sprite pohybImageIcon;
    [SerializeField] private Sprite smokeImageImageIcon;
    
    public static Sprite teplotaImage;
    public static Sprite vlhkostImage;
    public static Sprite oxidUholnatyImage;
    public static Sprite osvetlenieImage; 
    public static Sprite hlukImage;
    public static Sprite pohybImage;
    public static Sprite oxidUhličitýImage;
    public static Sprite smokeImage;
    
    private DataSnapshot snapshot;
    public DataSnapshot logSnapshot;
    private bool first = false;
    private bool waiting = false;

    public static List<Node>  allNodes = new List<Node>();


    private async void Awake()
    {
        teplotaImage = teplotaImageIcon;
        vlhkostImage = vlhkostImageIcon;
        oxidUholnatyImage = oxidUholnatyImageIcon;
        osvetlenieImage = osvetlenieImageIcon;
        hlukImage = hlukImageIcon;
        pohybImage = pohybImageIcon;
        oxidUhličitýImage = oxidUhličitýImageIcon;
        smokeImage = smokeImageImageIcon;
            
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        
        
        await GetAndShowData();
        await GetValueChanged();
    }

    private async Task GetAndShowData()
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
            if (!waiting)
            {
                Invoke( nameof(GetAndShowData),10);
                waiting = true;
            }
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
    

    private void ShowNodesDetail()
    {
        foreach (var node in allNodes)
        {
            if (node)
            {
                Destroy(node.gameObject);
            }
        }
        allNodes.Clear();
        nodeGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(900,0);
        
        
        
        int i = 0;
        foreach (var variable in snapshot.Children)
        {
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
                    actualSensor.SetImage();
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
            
            if (i % 2 == 0)
            {
                Vector2 size = nodeGrid.GetComponent<RectTransform>().sizeDelta;
                nodeGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,size.y + nodeGrid.GetComponent<GridLayoutGroup>().cellSize.y + nodeGrid.GetComponent<GridLayoutGroup>().spacing.y);
            }
            i++;
        }
        waiting = false;
        nodeGrid.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;

        ChangeOrderInSensorList();
    }

    private void ChangeOrderInSensorList()
    {
        foreach (var node in allNodes)
        {
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