using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float speed = 50.0f; //max speed of the camera
    public float cameraSensitivity = 0.25f; //recommended to be from 0-1; controls erratic movements
    public float actualSpeed = 0.0f; //keep between 0-1; for smoothing
    public float acceleration = 0.05f; //for smoothing
    public bool inverted = false; //for the inverted camera; can be controlled in inspector
    public bool smoothing = true; //for smoothing
     
    private Vector3 lastMouse = new Vector3(255, 255, 255); //for the mouse 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse movement
        lastMouse = Input.mousePosition - lastMouse;

        if (!inverted)
        {
            lastMouse.y = -lastMouse.y; //no longer inverted on y axis
        }
        lastMouse *= cameraSensitivity;

        //calculate Vector3, no rotation on x axis
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.y, transform.eulerAngles.y + lastMouse.x, 0);  
        transform.eulerAngles = lastMouse; //reset new angle
        lastMouse = Input.mousePosition;

        //WASD movement
        Vector3 direction = new Vector3(); //creates a new vector w/ (0,0,0)
        if (Input.GetKey(KeyCode.W)) //forward
        {
            direction.z += 1.0f; 
        }

        if (Input.GetKey(KeyCode.A)) //left
        {
            direction.x -= 1.0f; 
        }

        if (Input.GetKey(KeyCode.S)) //backwards
        {
            direction.z -= 1.0f;
        }

        if (Input.GetKey(KeyCode.D)) //right
        {
            direction.x += 1.0f; 
        }

        direction.Normalize(); //standardizes the vector
        transform.Translate(direction * actualSpeed * speed * Time.deltaTime); //moves and updates the camera!

        //smoothing indicates whether or not a key is bein pressed; movement activated
        if (direction != Vector3.zero) //movement
        {
            if (actualSpeed < 1)
            {
            actualSpeed += acceleration * Time.deltaTime;
            }
            else
            {
                actualSpeed = 1.0f; //reset
            }
        }
        else //decrease all movement
        {
            if (actualSpeed > 0)
            {
                actualSpeed -= acceleration * Time.deltaTime;
            }
            else
            {
                actualSpeed = 0.0f; //reset
            }
        }
    }

    //using GetAxis to get the mouse movement of each frame?
    private void OnGUI()
    {
        float axis = Input.GetAxis("Mouse X");
        float axisY = Input.GetAxis("Mouse Y");
        float axisScroll = Input.GetAxis("Mouse ScrollWheel");
        GUILayout.Box("Mouse movement: " + (axis + axisY + axisScroll));
    }
}
