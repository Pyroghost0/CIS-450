/* Caleb Kahn
 * ShopHoverDescription
 * Project 1
 * Notifies the main shop script that the mouse is hovering over a specififc button
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IObserver
{
    public Shop shop;
    public int itemNumber;
    public bool shopSide = true;
    private bool isPaused = false;
    //public bool mouseOver = false;

    void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>().RegisterObserver(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter " + itemNumber);
        //mouseOver = true;
        if (shop.itemSelected == -1 && (shopSide || GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().item != ItemType.Empty) && !isPaused)
        {
            shop.SlowText(shopSide ? shop.itemDescriptions[(int)shop.items[itemNumber]] : shop.itemDescriptions[(int)GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory[itemNumber]]);
            /*if (shop.items[itemNumber] == ItemType.Empty)
            {
                shop.SlowText("We're sold out... Read the sign.");
            }
            else
            {
                shop.SlowText(shop.itemDescriptions[(int)shop.items[itemNumber]]);
            }*/
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Leave " + itemNumber);
        //mouseOver = false;
        //shop.SlowText(shop.shoptenderMainTexts[(int)shop.shopType][Random.Range(0, shop.shoptenderMainTexts[(int)shop.shopType].Length)]);
        if (shop.itemSelected == -1 && (shopSide || GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().item != ItemType.Empty) && !isPaused)
        {
            shop.SlowText(shop.talkTexts[Random.Range(0, shop.talkTexts.Length)]);
        }
    }

    public void UpdateSelf(bool paused)
    {
        isPaused = paused;
    }
}

