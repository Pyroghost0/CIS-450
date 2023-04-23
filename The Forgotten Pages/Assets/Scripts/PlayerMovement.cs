/* Cooper Denault, Caleb Kahn, Anna Breuker
 * PlayerMovement
 * Project 5
 * Player can move around
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public bool sprinting = false;
    public float soundRadius = 2f;
    private float notMovingSoundRadius = 2f;
    private float walkingSoundRadius = 6f;
    private float SprintingSoundRadius = 12f;

    //variables for gravity
    public Vector3 velocity;
    public float gravity = -9.81f;

    //variables for checking if on ground
    public Transform behindPosition;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public bool isGrounded;

    //public float jumpHeight = 3f;
    public PlayerUpgrade playerUpgrades;
    public Light flashlight;

    public Image sanityBar;
    public Image jumpscareImage;
    public AudioSource jumpscareSound;

    public GameObject gameOverScreen;

    public AudioSource pickupSound;

    private void Start()
    {
        playerUpgrades = new PlayerAbility();
        //playerUpgrades = new PlayerFlashlight(playerUpgrades, flashlight);
        //playerUpgrades = new PlayerSprint(playerUpgrades, this);
    }


    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isInMemory)
        {
            //checking if the player is on the ground
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }



            //Get input
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float magnitude = Mathf.Sqrt(x * x + z * z);
            if (magnitude != 0)
            {
                x *= Mathf.Abs(x) / magnitude;
                z *= Mathf.Abs(z) / magnitude;
                soundRadius = sprinting ? SprintingSoundRadius : walkingSoundRadius;
            }
            else
            {
                soundRadius = notMovingSoundRadius;
            }
            //playerAnimation.SetFloat("X", Input.GetAxisRaw("Horizontal"));
            //playerAnimation.SetFloat("Y", Input.GetAxisRaw("Vertical"));

            controller.Move((transform.right * x + transform.forward * z) * speed * Time.deltaTime);

            playerUpgrades.UseAbillity();

            /*if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            //add gravity to velocity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);*/
        }
    }

    public void StartJumpscare(Sprite sprite)
    {
        StartCoroutine(Jumpscare(sprite));
    }

    IEnumerator Jumpscare(Sprite sprite)
    {
        jumpscareSound.Play();
        jumpscareImage.sprite = sprite;
        jumpscareImage.gameObject.SetActive(true);
        if (sanityBar.fillAmount == 0)
        {
            yield return new WaitForSecondsRealtime(.75f);
            float timer = 0f;
            while (timer < 1f)
            {
                jumpscareImage.color = new Color(1f, 1f, 1f, 1f - (timer));
                timer += Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }
            jumpscareImage.color = Color.white;
            jumpscareImage.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            yield return new WaitForSeconds(.75f);
            float timer = 0f;
            while (timer < 1f)
            {
                jumpscareImage.color = new Color(1f, 1f, 1f, 1f - (timer));
                timer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            jumpscareImage.color = Color.white;
            jumpscareImage.gameObject.SetActive(false);
        }
    }

    public void RemoveSanity(float amount)
    {
        if (sanityBar.fillAmount <= amount)
        {
            sanityBar.fillAmount = 0;
            Time.timeScale = 0f;
            gameOverScreen.SetActive(true);
            Debug.Log("Dead");
        }
        else
        {
            sanityBar.fillAmount -= amount;
        }
    }
}
