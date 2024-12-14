using System;
using BTAI;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Random = System.Random;

//Contains BT, Animations, and GameStart & GameOver implementation
public class FreddieBehavior : MonoBehaviour
{
    public Button start;
    public TMP_Text time; //for highscore in corner
    public TMP_Text gameOverUI;
    public TMP_Text restartUI;
    public Canvas startScreen; //UI

    public WanderBehavior wanderBehaviorScript; //reference to wander script
    public NavMeshAgent agent;
    public Transform player; //reference to player object
    public Transform NPC; //reference to Freddie

    public float detectionRange = 20f; //for chase
    public float speed = 3f; //initial chase speed, to increase over time
    public float timer;

    private bool isGameOver = false;
    private Root root = BT.Root(); //root node

    Vector3 target; //for wander
    Animator animator;
    bool isWalking = true; //for animations
    bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        startScreen.enabled = true;
        gameOverUI.enabled = false; //hide UI in the beginning
        restartUI.enabled = false; //hide UI 
        timer = Time.deltaTime;

        //Setup:
        Button btn = start.GetComponent<Button>(); //for startup screen
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NPC = transform; //attaches to Freddy
        agent = GetComponent<NavMeshAgent>();
        wanderBehaviorScript = GetComponent<WanderBehavior>();

        animator = GetComponent<Animator>();
        isWalking = animator.GetBool("isWalking");

        //Setup tree:
        Selector selector = BT.Selector(); //start the branch, choose sequence
        selector.OpenBranch();
        root.OpenBranch(selector); //connect root to selector node

        Sequence wanderSequence = BT.Sequence();
        Sequence chaseSequence = BT.Sequence();

        wanderSequence.OpenBranch(
            BT.Call(() => animator.SetBool("isWalking", true)), //set to walking
            BT.If(() => !InRange()).OpenBranch(BT.RunCoroutine(Wander)));

        chaseSequence.OpenBranch(
            BT.Call(() => animator.SetBool("isWalking", false)), //set to running
            BT.If(() => InRange()).OpenBranch(BT.RunCoroutine(Chase)));

        //add sequence to selector
        selector.OpenBranch(wanderSequence, chaseSequence);
    }

    //update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //restart 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //quit game 
        {
            print("Application quit");
            Application.Quit();
        }

        if (gameStarted) //start timer, won't chase until then
        {
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);

            time.text = "Time: " + string.Format("{0:0}:{1:00}", minutes, seconds); //display score
            timer += Time.deltaTime;
            speed += timer / 5; //freddie gets faster

            root.Tick(); //continue through tree 
        }

        if (isGameOver) //if true
        {
            StartCoroutine(GameOverSequence()); //for UI delays

            if (Input.GetKeyDown(KeyCode.R)) //restart 
            {
                gameStarted = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q)) //quit game 
            {
                print("Application quit");
                Application.Quit();
            }
        }
    }

    public void TaskOnClick() //when button is clicked
    {
        //hide canvas
        startScreen.enabled = false;
        gameStarted = true; //start game
    }


    bool InRange()
    {
        float distance = Vector3.Distance(player.position, NPC.position);

        return distance <= detectionRange; //if in range
    }

    //wander code from wander script
    private IEnumerator<BTState> Wander()
    {
        Vector3 randomPoint = NPC.position + (UnityEngine.Random.insideUnitSphere * 15f);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            target = hit.position;
        }

        agent.SetDestination(target); //move NPC

        //wait for agent to reach destination
        while (agent.remainingDistance > 0.1f)
        {
            yield return BTState.Continue;
        }



        yield return BTState.Success;
    }

    //action: chase (follow players last position)
    private IEnumerator<BTState> Chase()
    {
        while (InRange()) //if not in range 
        {
            //Debug.Log("Freddie is chasing you!!");

            agent.SetDestination(player.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //check if Freddie caught the player, game over condition
                if (Vector3.Distance(player.position, NPC.position) <= agent.stoppingDistance)
                {
                    //Debug.Log("Freddie has caught u!!");
                    isGameOver = true;
                    yield return BTState.Failure; 
                }
            }

            yield return BTState.Continue;
        }

        yield return BTState.Failure;
    }

    private IEnumerator GameOverSequence()
    {
        gameOverUI.enabled = true;

        yield return new WaitForSeconds(2.0f); //delay

        restartUI.enabled = true;
    }
}