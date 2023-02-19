using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    public ProgressBar rememberanceBar;
    public float rememberance = 100;

    //implement decorator pattern eventually

    //going to take the location of the player for proximity
    //or we could use observer pattern if we want to make it
    //overcomplicated but get the design pattern out of the way.
    
    public PlayerMovement player;
    public PlayerInventory playerInventory;
    public SpriteRenderer sprite;

    public GraveReaction reaction;

    public string[] flowerPreferences;
    public string[] flowerHates;

    public GameObject flowerLovedIndicator;
    public GameObject flowerHatedIndicator;

    //for the ghost ai

    //public Ghost ghost; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    // Update is called once per frame
    void Update()
    {
        rememberance -= Time.deltaTime*.25f;
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
