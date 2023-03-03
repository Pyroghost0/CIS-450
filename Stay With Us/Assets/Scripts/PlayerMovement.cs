using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Ok this should be the overall for the playermovement, ill do the best i can

    //public bool speedUp;
    private int initSpeed;
    public int speed;
    public Animator playerAnimation;

    public Rigidbody2D playerRb;

    public float horizontalInput;
    public float verticalInput;

    public PlotOfDirt plotOfDirt;

    public PlayerInventory playerInventory;

    public SpriteRenderer sprite;

    public AudioSource audioSource;
    public float stamina = 4f;
    public bool talking = false;
    public Ghost talkingGhost;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        initSpeed = speed;
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (talking)
            {
                talking = false;
                Time.timeScale = 1f;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>().canPause = true;
                talkingGhost.talkingComponent.SetActive(false);
            }
            else if (GameObject.FindGameObjectWithTag("Shoptender") == null || (GameObject.FindGameObjectWithTag("Shoptender").transform.position - transform.position).magnitude > 5f)
            {
                GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
                int minNum = 0;
                for (int i = 1; i < ghosts.Length; i++)
                {
                    if ((ghosts[i].transform.position - transform.position).magnitude < (ghosts[minNum].transform.position - transform.position).magnitude)
                    {
                        minNum = i;
                    }
                }
                if ((ghosts[minNum].transform.position - transform.position).magnitude < 3f)
                {
                    talking = true;
                    talkingGhost = ghosts[minNum].GetComponent<Ghost>();
                    Time.timeScale = 0f;
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>().canPause = false;
                    talkingGhost.talkingComponent.SetActive(true);
                    talkingGhost.Talk();
                }
            }
        }

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

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (stamina > 0f)
            {
                speed = initSpeed * 2;
                stamina -= Time.deltaTime;
            }
            else
            {
                speed = initSpeed;
            }
            /*if (!speedUp)
            {
                speed *= 2;
                speedUp = true;
                StartCoroutine(Stamia());
            }*/
        }
        else if (stamina < 4f)
        {
            speed = initSpeed;
            stamina += Time.deltaTime;
            if (stamina > 4f)
            {
                stamina = 4f;
            }
        }
        /*if (Input.GetKeyUp(KeyCode.Space))
        {
            if (speedUp)
            { 
                speed = speed/ 2;
                speedUp = false;            }
        }*/

        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    /*IEnumerator Stamia()
    {
        yield return new WaitForSeconds(4);
        if (speedUp)
        {
            speed = speed / 2;
            speedUp = false;
        }
    }*/

    private void FixedUpdate()
    {
        //playerRb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);
        float magnitude = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);
        if (magnitude != 0)
        {
            horizontalInput *= Mathf.Abs(horizontalInput) / magnitude;
            verticalInput *= Mathf.Abs(verticalInput) / magnitude;
        }
        playerAnimation.SetFloat("X", horizontalInput);
        playerAnimation.SetFloat("Y", verticalInput);
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
