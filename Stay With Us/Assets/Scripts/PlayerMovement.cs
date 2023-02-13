using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Ok this should be the overall for the playermovement, ill do the best i can


    public int speed;

    public Rigidbody2D playerRb;

    public float horizontalInput;
    public float verticalInput;

    public PlotOfDirt plotOfDirt;

    // Start is called before the first frame update
    void Start()
    {
        
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
                plotOfDirt.PlantFlower(0);
            }
        }

    }

    private void FixedUpdate()
    {
        playerRb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);


    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlantableDirt"))
        {

               plotOfDirt = other.GetComponent<PlotOfDirt>();

        }
    }
}
