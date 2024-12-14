using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    private NavMeshAgent agent; //connects agent from Unity
    public float range = 20.0f; //range from next point

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //starting point
        Vector3 point; //for method
        Vector3 randomPoint; //for storing the last found point 
        Vector3 currentPosition = transform.position;
        if (RandomPoint(currentPosition, 10, out point)) //checks if a point was found 
        {
            randomPoint = point;
            agent.SetDestination(randomPoint); //use NavMeshAgent.setDestination instead?
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check if agent has reached target / nearing target; adjust position
        if (agent.remainingDistance < 5.0f) 
        {
            Vector3 point; //for method
            Vector3 randomPoint; //for storing the last found point 
            Vector3 currentPosition = transform.position; //connected to skeleton component 

            if (RandomPoint(currentPosition, 10, out point)) //checks if a point was found 
            {
                randomPoint = point;
                agent.SetDestination(randomPoint); //use NavMeshAgent.setDestination instead?
            }
        }
    }


    //needs to query a random point; taken from Unity NavMesh.SamplePosition documentation
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + (Random.insideUnitSphere * range);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero; //if no point found
        return false;
    }
}
