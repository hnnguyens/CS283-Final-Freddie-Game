using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTAI;
using System;
using TMPro;

//talking cat NPC
public class BehaviorUnique : MonoBehaviour
{
    public float detectionRange = 2f; //range from NPC
    public TMP_Text Dialogue; //UI element from hierarchy
    public Transform player; //reference to player object
    public Transform NPC; //reference to cat

    private Root m_btRoot = BT.Root(); //root node

    //Start is called before the first frame update
    void Start()
    {
        Dialogue.enabled = false; //hide UI initially

        player = GameObject.FindGameObjectWithTag("Player").transform;
        NPC = transform;

        BTNode RangeCondition = BT.Condition(() => InRange());
        BTNode n1 = BT.RunCoroutine(() => (IEnumerator<BTState>)Say("Welcome traveler..")); 
        BTNode n2 = BT.RunCoroutine(() => (IEnumerator<BTState>)Say("..."));
        BTNode n3 = BT.RunCoroutine(() => (IEnumerator<BTState>)Say("You need to find the truth."));

        Sequence sequence = BT.Sequence();
        sequence.OpenBranch(RangeCondition, n1, n2, n3); //creates branch
        m_btRoot.OpenBranch(sequence); //connect

    }

    //Update is called once per frame, runs the tree
    void Update()
    {
        m_btRoot.Tick(); //iterate 
    }

    bool InRange() //checks if the player is close to the cat
    {
        float distance = Vector3.Distance(player.position, NPC.position);

        if (distance <= detectionRange)
        {
            return true;
        }

        return false;
    }

    IEnumerator<BTState> Say(String text)
    {
        //set text on UI
        Dialogue.enabled = true; //active 
        Dialogue.text = text;

        // Wait until the player clicks the mouse button (left click)
        while (!Input.GetMouseButtonDown(0))
        {
            yield return BTState.Continue; //wait one frame
        }

        Dialogue.enabled = false; //hide again

        yield return BTState.Success;
        
    }

}
