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
    public Collider2D ghostCollider;
    public int previousChoice = 2;
    private Collider2D walkableZone;
    private bool canEmotion = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GhostBehavior());
    }

    IEnumerator GhostBehavior()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        while (true)
        {
            int choice = (Random.Range(1, 4) + previousChoice) % 3;
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            if (choice == 0)//Go random direction
            {

            }
            else if (choice == 1)//Go away from wall
            {

            }
            else if (choice == 2)//Go to grave
            {

            }
            else if (choice == 3)//React to emotion
            {

            }
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
        yield return new WaitForSeconds(3f);
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
        }
    }
}
