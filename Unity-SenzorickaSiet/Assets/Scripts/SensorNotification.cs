using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorNotification : MonoBehaviour
{
    [SerializeField] private Text sensorQuantity;
    [SerializeField] private string sensorName;
    [SerializeField] private Text sensorUnit;
    [SerializeField] private Text sensorUnit2;


    [SerializeField]  private Image sensorImage;
    
    [SerializeField]  private Sprite teplotaImage;
    [SerializeField]  private Sprite vlhkostImage;

    [SerializeField]  private Sprite oxidUholnatyImage;
    [SerializeField]  private Sprite osvetlenieImage;
    [SerializeField]  private Sprite hlukImage;
    [SerializeField]  private Sprite pohybImage;


    

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

    
    
    
}
