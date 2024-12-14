using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Collection game implementation, restart and quit functions
public class FishCollectionGame : MonoBehaviour
{
    public Canvas instructions; //instructions UI
    public TMP_Text count; //collection UI
    public TMP_Text winUI; //win UI
    public TMP_Text restartUI; //restart UI
    public int collected = 0;

    // Start is called before the first frame update
    void Start()
    {
        instructions.enabled = true;
        winUI.enabled = false;

        count.text = "Fish collected: 0";
        UpdateScoreUI(); //method
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //press space to toggle 
        {
            instructions.enabled = !instructions.enabled;
        }

        if (collected >= 30) 
        {
            StartCoroutine(GameOverSequence());

            if (Input.GetKeyDown(KeyCode.R)) //restart 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q)) //quit game 
            {
                print("Application quit");
                Application.Quit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible")) //if they have collided
        {
            collected++;
            UpdateScoreUI();

            //animation here??

            other.gameObject.SetActive(false); //hides the collectible object 
        }
    }

    public void UpdateScoreUI()
    {
        count.text = "Fish collected: " + collected.ToString() + "/30";
    }

    private IEnumerator GameOverSequence()
    {
        winUI.enabled = true;

        yield return new WaitForSeconds(2.0f); //delay

        restartUI.enabled = true;
    }

 
}
