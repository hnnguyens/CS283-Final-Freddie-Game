using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class CollectionGame : MonoBehaviour
{
    public TMP_Text Score; //UI element from hierarchy
    public int count = 0; //starting score, to be increased

    // Start is called before the first frame update
    void Start()
    {
        Score.text = "Coins collected: 0";
        UpdateScoreUI(); //method
    }

    //used instead of Update()
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible")) //if they have collided
        {
            count++;
            UpdateScoreUI(); //updates the score in game
            //animation here
            other.gameObject.SetActive(false); //hides the collectible object 
        }
    }

    public void UpdateScoreUI()
    {
        Score.text = "Coins collected: " + count.ToString();
    }
}
