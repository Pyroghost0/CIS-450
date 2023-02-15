using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    public ProgressBar rememberanceBar;
    public float rememberance = 200;

    //implement decorator pattern eventually

    //going to take the location of the player for proximity
    //or we could use observer pattern if we want to make it
    //overcomplicated but get the design pattern out of the way.
    
    public PlayerMovement player;
    public PlayerInventory playerInventory;

    //for the ghost ai

    //public Ghost ghost; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        rememberance -= Time.deltaTime*.5f;
        if (rememberance <= 0)
        {
            rememberanceBar.current = 0;
        }
        else
        {
            rememberanceBar.current = rememberance;
        }
    }
}
