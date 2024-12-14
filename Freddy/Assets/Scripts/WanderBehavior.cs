using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BTAI;

public class WanderBehavior : MonoBehaviour
{
    public Transform wanderRange;  // Set to a sphere
    private Root m_btRoot = BT.Root();

    Vector3 target;

    void Start()
    {
        BTNode moveTo = BT.RunCoroutine(MoveToRandom); //coroutine

        Sequence sequence = BT.Sequence();
        sequence.OpenBranch(moveTo); //creates a node list

        m_btRoot.OpenBranch(sequence); //need this to connect from root
    }

    void Update()
    {
        m_btRoot.Tick(); //continue
    }

    public IEnumerator<BTState> MoveToRandom()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        Vector3 randomPoint = transform.position + (Random.insideUnitSphere * 10f);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            target = hit.position;
        }

        agent.SetDestination(target);

        // wait for agent to reach destination
        while (agent.remainingDistance > 0.1f)
        {
            yield return BTState.Continue;
        }

        yield return BTState.Success;
    }

}