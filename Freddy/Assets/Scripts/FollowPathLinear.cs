using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathLinear : MonoBehaviour
{
    public List<Transform> POIs; //a list of objects to traverse
    public float duration = 3.0f;
    private int currentIndex = 0; //the index in the list

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //reset currentIndex if out of bounds
        if (currentIndex >= POIs.Count)
        {
            currentIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space)) //activated with space
        {
            if (currentIndex < POIs.Count)
            {
                StartCoroutine(DoLerp());
            }            
        }
    }

    IEnumerator DoLerp() //the couroutine
    {
        //make sure it's within bounds
        if (currentIndex < POIs.Count - 1)
        {
            float timer = 0f; //timer reset
            Transform startPOI = POIs[currentIndex]; //pos 1
            Transform endPOI = POIs[currentIndex + 1]; //pos 2

            //moving from start position to end position
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float u = timer / duration;

                //lerp
                transform.position = Vector3.Lerp(startPOI.position, endPOI.position, u);            

                //rotation
                Vector3 direction = (endPOI.position - startPOI.position).normalized;
                if (direction != Vector3.zero) // Avoid zero direction
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation
                }

                yield return null; //wait for next frame
            }

            //ensures that character made it to the position
            transform.position = endPOI.position;
            transform.rotation = endPOI.rotation;

            //increase current index
            currentIndex++;
        }

        //reset if end reached
        if (currentIndex >= POIs.Count)
        {
            currentIndex = 0; //0
            transform.position = POIs[currentIndex].position; //return to original position
        }
    }
}
