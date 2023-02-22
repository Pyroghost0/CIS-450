using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Ok this should be the overall for the playermovement, ill do the best i can


    public int speed;

    public Rigidbody2D playerRb;

    public float horizontalInput;
    public float verticalInput;

    public PlotOfDirt plotOfDirt;

    public PlayerInventory playerInventory;

    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(ItemType.Flower, Vector3.zero, true);
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        if (Input.GetKeyDown(KeyCode.E))
        {
            //for player inventory im guessing
            Debug.Log("E button was pressed");
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            //interaction or abilities?
            Debug.Log("Q button was pressed");
            if (plotOfDirt != null)
            {
                if (playerInventory.item == ItemType.Flower)
                {
                    playerInventory.UseItem();
                    plotOfDirt.PlantFlower(0);
                }
            }
        }

        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    private void FixedUpdate()
    {
        //playerRb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);
        float magnitude = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);
        if (magnitude != 0)
        {
            horizontalInput *= Mathf.Abs(horizontalInput) / magnitude;
            verticalInput *= Mathf.Abs(verticalInput) / magnitude;
        }
        playerRb.velocity = new Vector2(horizontalInput, verticalInput) * speed;


    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlantableDirt"))
        {

            plotOfDirt = other.GetComponent<PlotOfDirt>();

        }
        else 
        { 
            plotOfDirt= null;
        }
    }
}
