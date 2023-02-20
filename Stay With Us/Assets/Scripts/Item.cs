/* Caleb Kahn
 * Item
 * Project 1
 * In game item that moves around and can be collected
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Empty = 0,
    Flower = 1
}

public abstract class Item : MonoBehaviour
{
    public ItemType itemType;
    public float floatCycleTime = .8f;
    public float floatCycleDistence = .2f;
    public SpriteRenderer sprite;
    protected float curentYpos = 0f;
    protected float timer = 0f;
    protected bool doublePickUp = false;

    void Start()
    {
        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    void Update()
    {
        if (!doublePickUp)
        {
            UpdateFunction();
        }
        /*if (transform.parent == null)
        {
            sprite.sortingOrder = (int)(transform.position.y * -10);
        }*/
    }

    protected abstract void UpdateFunction();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !doublePickUp)
        {
            if (itemType == ItemType.Flower)
            {
                if (collision.GetComponent<PlayerInventory>().AddInventory(ItemType.Flower))
                {
                    doublePickUp = true;
                    StartCoroutine(CollectionAnimation());
                    //Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator CollectionAnimation()
    {
        doublePickUp = true;
        sprite.sortingOrder = 9999;
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        Vector3 distance = transform.localPosition * -2f;
        timer = 0f;
        while (timer < .5f)
        {
            transform.localScale = Vector3.one * ((.5f - timer) / .5f);
            yield return new WaitForFixedUpdate();
            timer+=Time.deltaTime;
            transform.position += distance * Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
