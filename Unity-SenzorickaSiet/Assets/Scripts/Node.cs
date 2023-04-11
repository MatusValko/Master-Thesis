using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField] private string nodeName;
    [SerializeField] private Text nodeNameText;
    [SerializeField] private Button nodeDetail;
    [SerializeField] private DetailedNode detailedNode;
    
    [SerializeField] private List<Sensor> allSensors;
    
    
    
    
    void Start()
    {
        nodeDetail.onClick.AddListener(ShowNodeDetailScreen);
    }

    public void SetName(string value)
    {
        nodeName = value;
        nodeNameText.text = value; 
    }
    public string GetName()
    {
        return nodeName;
    }
    
    public void AddNewSensor(Sensor sensor)
    {
        allSensors.Add(sensor);
    }
    public List<Sensor> GetAllSensors()
    {
        return allSensors;
    }
    
    public void ShowNodeDetailScreen()
    {
        detailedNode = Resources.FindObjectsOfTypeAll<DetailedNode>()[0];
        if (!string.IsNullOrEmpty(nodeName))
        {
            detailedNode.ShowNodeDetailScreen(nodeName);
        }
        else
        {
            Debug.Log("String is empty!!");
        }
    }
    
    
    
    
}
