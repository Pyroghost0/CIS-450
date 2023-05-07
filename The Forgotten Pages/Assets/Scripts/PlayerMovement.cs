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
    private float walkingSoundRadius = 8f;
    private float SprintingSoundRadius = 15f;

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
    public bool freezeUsable = true;
    public RectTransform frozenOverlay;

    public Image sanityBar;
    public Image jumpscareImage;
    public AudioSource jumpscareSound;

    public GameObject gameOverScreen;
    public GameObject winScreen;

    public AudioSource pickupSound;
    public bool inJumpscare;

    public bool isWalking;
    public AudioSource walkingSound;

    public bool isSprinting;
    public AudioSource sprintingSound;

    private void Start()
    {
        playerUpgrades = new PlayerAbility();
        //playerUpgrades = new FlreezeAbility(playerUpgrades, this);
        //playerUpgrades = new PlayerFlashlight(playerUpgrades, flashlight);
        //playerUpgrades = new PlayerSprint(playerUpgrades, this);
    }

    public void MoreHidden()
    {
        notMovingSoundRadius = 2f;
        walkingSoundRadius = 6f;
        SprintingSoundRadius = 12f;
        TunnelMonsterSight.moreHidden = true;
    }

    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isInMemory && !inJumpscare)
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
                if (soundRadius == walkingSoundRadius)
                {
                    if (!isWalking)
                    {
                        isSprinting = false;
                        sprintingSound.Stop();
                        isWalking = true;
                        walkingSound.Play();
                        
                    }
                }
                else if (soundRadius == SprintingSoundRadius)
                {
                    if (!isSprinting)
                    {
                        isWalking = false;
                        walkingSound.Stop();
                        isSprinting = true;
                        sprintingSound.Play();
                    }
                }
            }
            else
            {
                isWalking = false;
                walkingSound.Stop();
                isSprinting = false;
                sprintingSound.Stop();
                soundRadius = notMovingSoundRadius;
            }
            //playerAnimation.SetFloat("X", Input.GetAxisRaw("Horizontal"));
            //playerAnimation.SetFloat("Y", Input.GetAxisRaw("Vertical"));

            controller.Move((transform.right * x + transform.forward * z) * speed * Time.deltaTime);

            playerUpgrades.UseAbillity();

            /*if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }*/
            //add gravity to velocity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
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
            Cursor.lockState = CursorLockMode.Confined;
            jumpscareImage.gameObject.SetActive(false);
            gameOverScreen.SetActive(true);
            Debug.Log("Dead");
        }
        else
        {
            sanityBar.fillAmount -= amount;
        }
    }

    public void Freeze()
    {
        StartCoroutine(FreezeCoroutine());
    }

    IEnumerator FreezeCoroutine()
    {
        freezeUsable = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().Freeze();
        }
        float timer = 0f;
        Image frozenImage = frozenOverlay.GetComponent<Image>();
        frozenOverlay.gameObject.SetActive(true);
        while (timer < 10f)
        {
            frozenOverlay.sizeDelta = new Vector2(960f, 540f) * (1f + (timer * timer / 300f));
            frozenImage.color = new Color(1f, 1f, 1f, 1f - (timer * timer / 100f));
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
        frozenOverlay.gameObject.SetActive(false);
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().Unfreeze();
            }
        }
        yield return new WaitForSeconds(20f);
        freezeUsable = true;
    }
}
