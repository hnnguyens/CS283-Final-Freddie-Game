using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionController : MonoBehaviour //first person POV 
{
    public Transform camera; //reference to camera object
    public float linearSpeed = 5.0f;
    public float turningSpeed = 8.0f;
    private bool isMoving = false; //for animation
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    Vector3 move;

    Animator animator; 
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        isMoving = animator.GetBool("isWalking"); //from the animator param
        move = new Vector3();

        if (camera == null)
        {
            camera = Camera.main.transform; //reference to main camera object
        }

    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        isMoving = false; //reset

        Vector3 forward = camera.forward;
        Vector3 right = camera.right;

        forward.y = 0;
        right.y = 0; 

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * vertical + right * horizontal;

        if (direction.magnitude > 0f || Input.GetKeyDown(KeyCode.W))
        {
            isMoving = true; //update 
        }

        if (controller.isGrounded)
        {
            verticalVelocity = 0f;
        }

        else
        {
            verticalVelocity += gravity * Time.deltaTime; //apply gravity so we don't fly!
        }

        move = direction * linearSpeed;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        animator.SetBool("isWalking", isMoving); //update animator bool

    }
}