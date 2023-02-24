using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Ok this should be the overall for the playermovement, ill do the best i can

    public bool speedUp;
    public int speed;

    public Rigidbody2D playerRb;

    public float horizontalInput;
    public float verticalInput;

    public PlotOfDirt plotOfDirt;

    public PlayerInventory playerInventory;

    public SpriteRenderer sprite;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        /*if (Input.GetKeyDown(KeyCode.E))
        {
            //for player inventory im guessing
            Debug.Log("E button was pressed");
        }*/


        if (Input.GetKeyDown(KeyCode.Q))
        {
            //interaction or abilities?
            //Debug.Log("Q button was pressed");
            if (plotOfDirt != null)
            {
                if ((int)playerInventory.item >= 2 && (int)playerInventory.item <= 6)
                {
                    plotOfDirt.PlantFlower((int)playerInventory.item - 2);
                    playerInventory.UseItem();

                    audioSource.Play();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!speedUp)
            {
                speed *= 2;
                speedUp = true;
                StartCoroutine(Stamia());
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (speedUp)
            { 
                speed = speed/ 2;
                speedUp = false;            }
        }

        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    IEnumerator Stamia()
    {
        yield return new WaitForSeconds(4);
        if (speedUp)
        {
            speed = speed / 2;
            speedUp = false;
        }
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
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlantableDirt") && plotOfDirt != null && other.gameObject == plotOfDirt.gameObject)
        {
            plotOfDirt = null;
        }
    }
}
