/* Caleb Kahn
 * Ghost
 * Project 1
 * Ghosts that have graves and emotions, they can potentially help the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Grave grave;
    public GameObject ghostBar;
    public GameObject floatingImagePrefab;
    public Sprite[] emotionSprites;
    public Collider2D ghostBigRadiusCollider;
    public Collider2D ghostCollider;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    private int previousChoice = 2;
    private Collider2D walkableZone;
    private bool canEmotion = true;

    // Start is called before the first frame update
    void Start()
    {
        walkableZone = GameObject.FindGameObjectWithTag("GameController").GetComponent<PolygonCollider2D>();
        StartCoroutine(GhostBehavior());
    }

    IEnumerator GhostBehavior()
    {
        //yield return new WaitForSeconds(Random.Range(1f, 2f));
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Pow(Random.Range(1.4f, 3f), 2f));//2f - 9f
            int choice = (Random.Range(1, 3) + previousChoice) % 3;
            if (choice == 0)//Go random direction
            {
                Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 40f;
                float timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * Time.deltaTime);
                }
                for (int i = 0; i < 20; i++)
                {
                    yield return new WaitForSeconds(Random.Range(.1f, .5f));//2f - 10f
                    if (!walkableZone.IsTouching(ghostCollider))
                    {
                        break;
                    }
                }
                timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * -Time.deltaTime);
                }
                rigidbody.velocity = Vector2.zero;
            }
            /*else if (choice == 1)//Go away from wall
            {
                Debug.Log(walkableZone.ClosestPoint(transform.position));//Wont work because your inside it
                Vector2 dir = (new Vector2(transform.position.x, transform.position.y) - walkableZone.ClosestPoint(transform.position)).normalized * 40f;
                float timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * Time.deltaTime);
                }
                for (int i = 0; i < 20; i++)
                {
                    yield return new WaitForSeconds(Random.Range(.1f, .5f));//2f - 10f
                    if (!walkableZone.IsTouching(ghostCollider))
                    {
                        break;
                    }
                }
                timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * -Time.deltaTime);
                }
                rigidbody.velocity = Vector2.zero;
            }*/
            else if (choice == 1)//Go to grave
            {
                Vector2 dir = new Vector2(grave.gameObject.transform.position.x - transform.position.x, grave.gameObject.transform.position.y - transform.position.y).normalized * 40f;
                float timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * Time.deltaTime);
                }
                for (int i = 0; i < 20; i++)
                {
                    yield return new WaitForSeconds(Random.Range(.1f, .5f));//2f - 10f
                    if (!walkableZone.IsTouching(ghostCollider))
                    {
                        break;
                    }
                }
                timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * -Time.deltaTime);
                }
                rigidbody.velocity = Vector2.zero;
            }
            else if (choice == 2)//React to emotion
            {
                if (grave.rememberance > 75f)//Give player ectoplasm
                {
                    Vector2 dir = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y - transform.position.y).normalized * 40f;
                    float timer = 0f;
                    while (timer < 1f)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                        rigidbody.AddForce(dir * Time.deltaTime);
                    }
                    Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
                    for (int i = 0; i < 20; i++)
                    {
                        yield return new WaitForSeconds(Random.Range(.1f, .5f));//2f - 10f
                        if (!walkableZone.IsTouching(ghostCollider))
                        {
                            break;
                        }
                        else if (ghostBigRadiusCollider.IsTouching(playerCollider))
                        {
                            //GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(ItemType.Ectoplasm, transform.position, true);
                            break;
                        }
                    }
                    timer = 0f;
                    while (timer < 1f)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                        rigidbody.AddForce(dir * -Time.deltaTime);
                    }
                    rigidbody.velocity = Vector2.zero;
                }
                else if (grave.rememberance > 50f)//Sit around more
                {
                    yield return new WaitForSeconds(Random.Range(2f, 4f));
                }
                else if (grave.rememberance > 25)//Go away from player
                {
                    Vector2 dir = new Vector2(transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x, transform.position.y - GameObject.FindGameObjectWithTag("Player").transform.position.y).normalized * 40f;
                    float timer = 0f;
                    while (timer < 1f)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                        rigidbody.AddForce(dir * Time.deltaTime);
                    }
                    Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
                    for (int i = 0; i < 20; i++)
                    {
                        yield return new WaitForSeconds(Random.Range(.1f, .5f));//2f - 10f
                        if (!walkableZone.IsTouching(ghostCollider))
                        {
                            break;
                        }
                    }
                    timer = 0f;
                    while (timer < 1f)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                        rigidbody.AddForce(dir * -Time.deltaTime);
                    }
                    rigidbody.velocity = Vector2.zero;
                }
                else
                {//Steal / destroy flowers
                    /*Vector2 dir = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y - transform.position.y).normalized * 40f;
                    float timer = 0f;
                    while (timer < 1f)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                        rigidbody.AddForce(dir * Time.deltaTime);
                    }
                    Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
                    for (int i = 0; i < 20; i++)
                    {
                        yield return new WaitForSeconds(Random.Range(.1f, .5f));//2f - 10f
                        if (!walkableZone.IsTouching(ghostCollider))
                        {
                            break;
                        }
                        else if (ghostBigRadiusCollider.IsTouching(playerCollider))
                        {
                            //GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(ItemType.Ectoplasm, transform.position, true);
                            break;
                        }
                    }
                    timer = 0f;
                    while (timer < 1f)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                        rigidbody.AddForce(dir * -Time.deltaTime);
                    }
                    rigidbody.velocity = Vector2.zero;*/
                }
            }
            if (!walkableZone.IsTouching(ghostCollider))
            {
                Vector2 dir = new Vector2(0f - transform.position.x, 0f - transform.position.y).normalized * 40f; ;
                float timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * Time.deltaTime);
                }
                Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
                for (int i = 0; i < 20; i++)
                {
                    yield return new WaitForSeconds(Random.Range(.1f, .5f));//2f - 10f
                    if (!walkableZone.IsTouching(ghostCollider))
                    {
                        break;
                    }
                }
                timer = 0f;
                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                    rigidbody.AddForce(dir * -Time.deltaTime);
                }
                rigidbody.velocity = Vector2.zero;
            }
            previousChoice = choice;
        }
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    IEnumerator Emotion()
    {
        Instantiate(floatingImagePrefab, ghostBar.transform.position, ghostBar.transform.rotation).GetComponent<FloatingImage>().sprite.sprite = grave.rememberance > 75f ? emotionSprites[0] : grave.rememberance > 50f ? emotionSprites[1] : grave.rememberance > 25f ? emotionSprites[2] : emotionSprites[3];
        canEmotion = false;
        yield return new WaitForSeconds(8f);
        canEmotion = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canEmotion)
            {
                StartCoroutine(Emotion());
            }
            ghostBar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ghostBar.SetActive(false);
        }
    }
}
