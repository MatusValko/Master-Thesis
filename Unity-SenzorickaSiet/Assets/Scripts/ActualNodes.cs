using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualNodes : MonoBehaviour
{
    //[SerializeField] private GameObject panel;
    
    // Start is called before the first frame update
    private bool right = true;
    private bool left = false;
    private bool moving = false;
    private Vector3 base_position;
    [SerializeField] private float speed = 1.5f;

    void Start()
    {
        base_position = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (right)
            {
                var transformPosition = transform.position;
                //var transformPosition = transform1.position;
                transformPosition.x -= speed * Time.deltaTime;
                transform.position = transformPosition;
                
                if (transform.position.x <= 0)
                {
                    transformPosition.x = 0;
                    transform.position = transformPosition;
                    right = false;
                    left = true;
                    moving = false;
                }
            }
            else if (left)
            {
                var transformPosition = transform.position;
                transformPosition.x += speed * Time.deltaTime;
                transform.position = transformPosition;
                //Debug.Log("transform.positionX: "+ transform.position.x + "Screen Width: " + Screen.width);
                if (transform.position.x >= base_position.x)
                {
                    transformPosition.x = base_position.x;
                    transform.position = transformPosition;
                    right = true;
                    left = false;
                    moving = false;
                }
            }
        }
    }
    
    
    public void Move()
    {
        if (moving) return;
        moving = true;
    }
    
    
    
}
