using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject fish; //game object to spawn; prefab template 
    public float spawnRange = 20f; //radius from the spawner center
    public int max = 50; //max number of objects to spawn

    private GameObject[] objects; //for keeping track


    // Start is called before the first frame update; similar to C# game and spawning platforms
    void Start()
    {
        objects = new GameObject[max]; //10
        
        //initialize and spawn collectibles
        for (int i = 0; i < max; i++)
        {
            SpawnCollectible(i); //calls method
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //for spawning objects
    private void SpawnCollectible(int index)
    {
        //generate random 2D position in the circular area
        Vector2 randomCircle = Random.insideUnitCircle * spawnRange;

        Vector3 randomPosition = new Vector3(
            transform.position.x + randomCircle.x, //random
            transform.position.y + 1,             //fixed y height
            transform.position.z + randomCircle.y //random
        );

        GameObject collectible = Instantiate(fish, randomPosition, Quaternion.identity); 

        collectible.SetActive(true);
        objects[index] = collectible;
    }
}
