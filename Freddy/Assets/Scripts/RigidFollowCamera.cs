using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidFollowCamera : MonoBehaviour
{
    public Transform target; //assigned to character
    public float hDistance; 
    public float vDistance;
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    private Vector3 smoothVelocity;
    private Vector3 offset; //position from player 

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        offset = new Vector3(0, vDistance, -hDistance);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 eye = target.position - target.forward * hDistance + target.up * vDistance;

        //The direction the camera should point is from the target to the camera position
        Vector3 cameraForward = target.position - eye;

        //Set the camera's position and rotation with the new values
        //This code assumes that this code runs in a script attached to the camera
        transform.position = eye;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }

}
