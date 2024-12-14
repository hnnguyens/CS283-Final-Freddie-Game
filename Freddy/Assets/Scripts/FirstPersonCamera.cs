using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    public Transform player; //reference to player object
    public float xRotation = 0f; //for pitch

    //Start is called before the first frame update
    void Start()
    {
    }

    //Update is called once per frame
    void Update() //to work in combination with player controls 
    {
        //mouse input from user
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //vertical rotation (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //to avoid flipping

        //apply camera pitch
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 

        //apply horizontal rotation (yaw)
        player.Rotate(Vector3.up * mouseX);
    }
}
