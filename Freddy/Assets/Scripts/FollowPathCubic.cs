using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathCubic : MonoBehaviour
{
    public Transform p0; //start point
    public Transform p1;
    public Transform p2;
    public Transform p3; //end point
    public float duration = 2.0F;
    public bool DeCasteljau = false; //if true use calculation

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(CubicPath());
        }
    }

    IEnumerator CubicPath()
    {
        if (DeCasteljau == true)
        {
            //algortithm here
        }

        else
        {
            timer = 0; //reset

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration; //calculate t

                if (t > 1f) //condition to check if it exceeds 1
                {
                    t = 1f; //reset 
                }

                Vector3 position = CalculateBezier(t, p0.position, p1.position, p2.position, p3.position);
                transform.position = position;

                //rotation / direction
                Vector3 direction = CalculateTangent(t, p0.position, p1.position, p2.position, p3.position);

                if (direction != Vector3.zero) 
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation
                }

                yield return null; //wait for next frame 
            }
        }

        //ensures it arrives at position
        transform.position = p3.position;
        transform.rotation = Quaternion.LookRotation(p3.position - p2.position);
    }

    private Vector3 CalculateBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;    
        Vector3 bezier = -3 * u * u * p0 + //first term
                          3 * u * (1 - 2 * t) * p1 + //second term
                          3 * t * (2 * u) * p2 + //third term
                          3 * t * t * p3; //fourth term

        return bezier;
    }

    //tangent determines the direction in which a vector heads to a point
    private Vector3 CalculateTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        Vector3 tangent = -3 * u * u * p0 +
                          3 * u * (1 - 2 * t) * p1 +
                          3 * t * (2 * u) * p2 +
                          3 * t * t * p3;

        return tangent.normalized; //vector
    }
}
