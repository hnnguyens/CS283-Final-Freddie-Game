using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float linearSpeed = 2.0f;
    public float turningSpeed = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //reads current input / location of char      
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //WASD movement
        if (Input.GetKey(KeyCode.W))
        {
            direction.z += 1.0f; //forward 1 space
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1.0f;
            transform.forward = direction;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction.z -= 1.0f;
            transform.forward = direction;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1.0f;
            transform.forward = direction;
        }

        direction.Normalize(); //standardizes the vector!

        if (direction.magnitude > 0.1f) //checks if the character is moving, if yes then Vector != (0, 0, 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //slerp instead of lerp for a smoother transition
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
            transform.Translate(direction * linearSpeed * Time.deltaTime, Space.World); //movement, relevant to the world
        }                     
    }
}
