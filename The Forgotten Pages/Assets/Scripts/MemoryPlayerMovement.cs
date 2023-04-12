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

    // Start is called before the first frame update
    void Start()
    {
        //assign variables
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isInMemory)
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
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}
