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

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public bool despawn = true;
    public float despawnTime = 15f;
    public float despawnFadeTime = 2f;
    public float floatCycleTime = .8f;
    public float floatCycleDistence = .2f;
    public SpriteRenderer sprite;
    private float curentYpos = 0f;
    private float timer = 0f;
    private bool DoublePickUp = false;


    void Update()
    {
        timer += Time.deltaTime;
        float distence = floatCycleDistence * -Mathf.Sin(timer / floatCycleTime * Mathf.PI);
        transform.position += new Vector3(0f, curentYpos - distence, 0f);
        curentYpos = distence;
        if (despawn && timer > despawnTime - despawnFadeTime)
        {
            if (timer > despawnTime)
            {
                Destroy(gameObject);
            }
            else
            {
                sprite.color = new Color(1f, 1f, 1f, (despawnTime - timer) / despawnFadeTime);
            }
        }
        sprite.sortingOrder = (int)(transform.position.y * -10);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !DoublePickUp)
        {
            if (itemType == ItemType.Flower)
            {
                if (collision.GetComponent<PlayerInventory>().AddInventory(ItemType.Flower))
                {
                    DoublePickUp = true;
                    Destroy(gameObject);
                }
            }
            /*if (itemType == ItemType.Money50)
            {
                PlayerBehaviour player = collision.GetComponent<PlayerBehaviour>();
                player.money += PlayerPrefs.GetInt("MoneyGained");
                //money check debug
                //Debug.Log(player.money + "cash item");
                //Debug.Log(PlayerPrefs.GetInt("Money") + "prefs item");
                player.moneyText.text = "$" + ((int)(player.money / 100)) + "." + (player.money % 100 == 0 ? "00" : player.money / 10 % 10 == 0 ? "0" + (player.money % 100).ToString() : (player.money % 100).ToString());
                DoublePickUp = true;
                Destroy(gameObject);
            }
            else if (itemType == ItemType.Molotov)
            {
                GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
                if (gc.AddInventory(ItemType.Molotov))
                {
                    DoublePickUp = true;
                    Destroy(gameObject);
                }
            }*/
        }
    }
}
