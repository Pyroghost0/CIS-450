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

public class ShopHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Shop shop;
    public int itemNumber;
    //public bool mouseOver = false;


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter " + itemNumber);
        //mouseOver = true;
        if (shop.items[itemNumber] == ItemType.Empty)
        {
            shop.SlowText("We're sold out... Read the sign.");
        }
        else
        {
            shop.SlowText(shop.itemTexts[(int)shop.items[itemNumber]]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Leave " + itemNumber);
        //mouseOver = false;
        //shop.SlowText(shop.shoptenderMainTexts[(int)shop.shopType][Random.Range(0, shop.shoptenderMainTexts[(int)shop.shopType].Length)]);
        shop.SlowText(shop.talkTexts[Random.Range(0, shop.talkTexts.Length)]);
    }
}

