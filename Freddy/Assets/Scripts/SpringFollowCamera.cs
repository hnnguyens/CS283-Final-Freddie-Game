using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFollowCamera : MonoBehaviour
{
    public Transform target; //assign to character in Unity IDE
    public float hDistance = 5.0f;
    public float vDistance = 1.0f;
    public float dampConstant = 10f; //dampening factor; smooths speed gradually
    public float springConstant = 5f; //spring factor;

    public Vector3 velocity = Vector3.zero;
    public Vector3 actualPos;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the camera's position at the ideal position
        Vector3 tPos = target.position;
        Vector3 tForward = target.forward;
        Vector3 tUp = target.up;

        actualPos = tPos - tForward * hDistance + tUp; // * vDistance;
        transform.position = actualPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() //for smoother transitions
    {
        //target's position and orientation
        Vector3 tPos = target.position;
        Vector3 tForward = target.forward;
        Vector3 tUp = target.up;

        //compute the ideal camera position
        Vector3 idealEye = tPos - tForward * hDistance + tUp; // * vDistance;

        //compute the displacement and spring acceleration
        Vector3 displacement = actualPos - idealEye;
        Vector3 springAccel = (-springConstant * displacement) - (dampConstant * velocity);

        //update velocity and position using spring acceleration
        velocity += springAccel * Time.deltaTime;
        actualPos += velocity * Time.deltaTime;

        //compute the direction the camera should point
        Vector3 cameraForward = tPos - actualPos;

        //update the camera's position and rotation
        transform.position = actualPos;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }
}
