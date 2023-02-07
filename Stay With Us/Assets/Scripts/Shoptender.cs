/* Caleb Kahn
 * Shoptender
 * Project 1
 * The character that leads to the shop in game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shoptender : MonoBehaviour
{
    //public bool gameShop = true;
    public GameObject rightClickPromp;
    public bool inShop = false;
    public bool inRadius = false;
    public ItemType[] items = new ItemType[3];
    public int[] itemAmount = new int[3];
    //public ShopType shopType;
    //public MoneyType[] itemPriceTypes = new MoneyType[3];
    public int[] itemPrices = new int[3];
    //public SpriteRenderer spriteRenderer;
    //public AudioSource shopBell;


    /*void Start()
    {
        if (gameShop)
        {
            spriteRenderer.sortingOrder = (int)(transform.position.y * -10);
        }
    }*/

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !inShop && inRadius)
        {
           StartCoroutine(OpenShop());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRadius = true;
            rightClickPromp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rightClickPromp.SetActive(false);
            inRadius = false;
        }
    }

    IEnumerator OpenShop()
    {
        //shopBell.Play();
        Time.timeScale = 0f;
        //if (gameShop)
        //{
            //GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isPaused = true;
        //}
        inShop = true;
        AsyncOperation ao = SceneManager.LoadSceneAsync("Shop", LoadSceneMode.Additive);
        yield return new WaitUntil(() => ao.isDone);
        GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>().StartShop(this);
    }

    public void CloseShop()
    {
        StartCoroutine(CloseShopCoroutine());
    }

    IEnumerator CloseShopCoroutine()
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync("Shop");
        yield return new WaitUntil(() => ao.isDone);
        Time.timeScale = 1f;
        //if (gameShop)
        //{
            //GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isPaused = false;
        //}
        yield return new WaitForSeconds(0.25f);
        inShop = false;
    }
}
