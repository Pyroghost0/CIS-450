/* Cooper Denault, Caleb Kahn
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

    public float jumpHeight = 3f;
    public PlayerUpgrade playerUpgrades;
    public Light flashlight;

    public Image sanityBar;


    private void Start()
    {
        playerUpgrades = new PlayerAbility();
        playerUpgrades = new PlayerFlashlight(playerUpgrades, flashlight);
        playerUpgrades = new PlayerSprint(playerUpgrades, this);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            RemoveSanity(other.GetComponent<Enemy>().sanityDamage);
            Debug.Log("JumpScare");
            Destroy(other.gameObject);
        }
    }

    public void RemoveSanity(float amount)
    {
        if (sanityBar.fillAmount <= amount)
        {
            sanityBar.fillAmount = 0;
            Debug.Log("Dead");
        }
        else
        {
            sanityBar.fillAmount -= amount;
        }
    }
}
