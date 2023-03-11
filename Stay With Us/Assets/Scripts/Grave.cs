/* Anna Breuker
 * Grave
 * Project 1
 * Grave that slowly decrases rememberance over time
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public SpriteRenderer ghostSpriteRenderer;
    public Sprite[] ghostSprites;

    public GraveReaction reaction;

    public string[] flowerPreferences;
    public string[] flowerHates;

    public float rememberanceRate = .5f;
    public GameObject flowerLovedIndicator;
    public GameObject flowerHatedIndicator;

    public GameObject graveName;
    public TextMeshProUGUI graveNameText;
    public TextMeshProUGUI ghostNameText;

    //for the ghost ai

    //public Ghost ghost; 

    // Start is called before the first frame update
    void Start()
    {
        graveNameText.text = ghostNameText.text + "'s Grave";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    // Update is called once per frame
    void Update()
    {
        rememberance -= Time.deltaTime*rememberanceRate;
        if (rememberance < 33.3f)
        {
            ghostSpriteRenderer.sprite = ghostSprites[2];
            if (rememberance < 0f)
            {
                rememberance = 0f;
            }
        }
        else if (rememberance < 66.6f)
        {
            ghostSpriteRenderer.sprite = ghostSprites[1];
        }
        else
        {
            ghostSpriteRenderer.sprite = ghostSprites[0];
            if (rememberance > 100f)
            {
                rememberance = 100f;
            }
        }
        rememberanceBar.current = rememberance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            graveName.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //ghostBar.SetActive(false);
            Collider2D[] pc = collision.GetComponents<Collider2D>();
            if (!GetComponent<Collider2D>().IsTouching(pc[0]) && !GetComponent<Collider2D>().IsTouching(pc[1]))
            {
                graveName.SetActive(false);
            }
        }
    }
}
