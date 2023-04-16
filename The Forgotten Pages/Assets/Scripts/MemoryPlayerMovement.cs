using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public float horizontal;
    public float moveLimiter = 0.7f;
    public float movementSpeed = 3f;

    public bool canMove;
    public bool isMoving;
    public bool sprintUnlocked;
    public bool isSprinting;
    public bool hasFallen;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        //assign variables
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            movementSpeed = 0;
        }
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isInMemory && canMove)
        {
            //player movement
            horizontal = Input.GetAxis("Horizontal");
            if (horizontal != 0)
            {
                horizontal *= moveLimiter;
            }
            body.velocity = new Vector3(horizontal * movementSpeed, 0);
            if (horizontal > .01)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontal < -.01)
            {
                spriteRenderer.flipX = true;
            }

            if (horizontal > 0.1 || horizontal < -0.1)
            {
                animator.SetBool("isMoving", true);
                isMoving = true;
            }
            else
            {
                animator.SetBool("isMoving", false);
                isMoving = true;
            }

            if (sprintUnlocked)
            {
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    isSprinting = true;
                    animator.speed = 2;
                    movementSpeed = 6;
                }
                else
                {
                    isSprinting = false;
                    animator.speed = 1;
                    movementSpeed = 3;
                }
            }
        }
    }

    public void Fall()
    {
        canMove = false;
        isMoving = false;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        transform.position += new Vector3(0, -.3f, 0);
        body.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
    }

    public void GetBackUp()
    {
        canMove = true;
        //isMoving = false;
        transform.rotation = Quaternion.Euler(0,0,0);
        transform.position += new Vector3(0, .3f, 0);
    }
}
