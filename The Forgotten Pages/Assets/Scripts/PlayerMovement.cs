/* Cooper Denault, Caleb Kahn
 * PlayerMovement
 * Project 5
 * Player can move around
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    private float initSpeed;
    private bool sprinting = false;
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


    private void Start()
    {
        initSpeed = speed;
        playerUpgrades = new PlayerAbility();
        playerUpgrades = new PlayerFlashlight(playerUpgrades, flashlight);
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
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                sprinting = true;
                speed = initSpeed * 2;
            }
            else
            {
                sprinting = false;
                speed = initSpeed;
            }
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
}
