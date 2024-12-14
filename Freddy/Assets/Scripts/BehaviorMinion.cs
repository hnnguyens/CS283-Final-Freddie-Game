using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using BTAI;
using TMPro;
using UnityEngine.AI;
using Random = System.Random;

//skeleton (essentially a patroller); wandering attached 
public class BehaviorMinion : MonoBehaviour
{
    public TMP_Text attackUI;
    public WanderBehavior wanderBehaviorScript; //reference to wander script
    public NavMeshAgent agent;
    public Transform player;
    public Transform NPC;
    public Transform boundary; //cube for home area
    public Transform retreatSpot; //sphere for npc area  
    public float detectionRange = 20f;
    public float speed = 3f; 
    bool InBoundary = false;
    bool IsAttacking = false;

    Vector3 target;
    private Root m_btRoot = BT.Root(); //root node

    // Start is called before the first frame update
    void Start()
    {
        attackUI.enabled = false; //hide UI

        //Setup:
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NPC = transform;
        agent = GetComponent<NavMeshAgent>();
        boundary = GameObject.FindGameObjectWithTag("Boundary").transform;
        retreatSpot = GameObject.FindGameObjectWithTag("Origin").transform;
        wanderBehaviorScript = GetComponent<WanderBehavior>(); //get wander component 

        Selector selector = BT.Selector(); //start the branch, choose sequence 
        selector.OpenBranch(); 
        m_btRoot.OpenBranch(selector); //connect the branch

        //sequence attackSequence = BT.Sequence();
        Sequence attackSequence = BT.Sequence();
        Sequence chaseSequence = BT.Sequence(); 
        Sequence retreatSequence = BT.Sequence(); 
        Sequence wanderSequence = BT.Sequence();

        // Setup nodes
        attackSequence.OpenBranch(
            BT.If(() => InRange()).OpenBranch( //if in range
            BT.RunCoroutine(() => Attack("You've been hit!"))) //attack action
        );

        chaseSequence.OpenBranch(
            BT.If(InRange).OpenBranch( //if in range
            BT.RunCoroutine(Chase)) //chase action
        );

        retreatSequence.OpenBranch(
            BT.If(() => InBoundary).OpenBranch( //if in boundary
            BT.RunCoroutine(Retreat)) //retreat action
        );

        wanderSequence.OpenBranch(
            BT.If(() => !InRange() && !InBoundary).OpenBranch(   
            BT.RunCoroutine(Wander)) //use the new Wander coroutine
        );

        //add sequence to selector
        selector.OpenBranch(attackSequence, chaseSequence, retreatSequence, wanderSequence);

    }

    //update is called once per frame
    void Update()
    {
        Collider c = boundary.GetComponent<Collider>();
        Bounds bounds = c.bounds; //of collider
        InBoundary = bounds.Contains(NPC.position); //returns boolean

        m_btRoot.Tick(); //continue through tree 
    }

    bool InRange()
    {
        float distance = Vector3.Distance(player.position, NPC.position);

        if (distance <= detectionRange)
        {
            return true;
        }

        return false;
    }

    //action: attack node
    private IEnumerator<BTState> Attack(String text)
    {
        if (!InRange())
        {
            attackUI.enabled = false;
            yield return BTState.Failure;
        }

        IsAttacking = true; 
        attackUI.enabled = true;
        attackUI.text = text;

        yield return BTState.Success;
    }

    //action: chase (follow players last position)
    private IEnumerator<BTState> Chase()
    {
        if (!InRange()) //if not in range 
        {
            yield return BTState.Failure;
        }

        agent.SetDestination(player.position);

        yield return BTState.Success;
    }

    //action: retreat (to a specific transform)
    private IEnumerator<BTState> Retreat()
    {
        if (!InBoundary) //if not in boundary
        {
            yield return BTState.Failure;
        }
 
        agent.SetDestination(retreatSpot.position);

        yield return BTState.Continue;
    }

    //wander code from Wander script 
    private IEnumerator<BTState> Wander()
    {
        Vector3 randomPoint = NPC.position + (UnityEngine.Random.insideUnitSphere * 10f);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            target = hit.position;
        }

        agent.SetDestination(target);

        //wait for agent to reach destination
        while (agent.remainingDistance > 0.1f)
        {
            yield return BTState.Continue;
        }

        yield return BTState.Success;
    }
}
