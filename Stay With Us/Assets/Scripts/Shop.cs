/* Caleb Kahn
 * Shop
 * Project 1
 * Operations of the shop i.e. buying items and adding to inventory
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    Empty = 0,
    Flower = 1
}
/*
public enum ShopType
{
    Reaper = 0,
}

public enum MoneyType
{
    Money = 0,
}*/

public class Shop : MonoBehaviour
{
    //public ShopType shopType;
    //public TextMeshProUGUI shopName;
    //public Sprite[] shoptenderSprites;
    //public Image shoptenderImage;
    //public Sprite[] moneyImageType;
    //public Image[] buttonMoneyImages;
    //public MoneyType[] itemPriceTypes;
    public TextMeshProUGUI shopMainText;
    public Sprite[] itemTypeSprites;
    public Image[] itemImages;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonPriceTexts;
    public GameObject[] buyButtons;
    public GameObject[] soldOutButtons;
    private Shoptender shoptender;
    public ItemType[] items;
    public int[] itemAmount;
    private int[] itemPrices;
    private Coroutine textCoroutine;
    public AudioSource buyAudio;

    //private float[] textSpeeds = { .03f, .02f, .016f, .025f };
    private string[] shopNames = { "The Bar", "Gun Shop", "Black Market", "Tailor" };
    public string[] itemTexts = { "Sold out, read the sign...", "Some bullets..."};
    public string[] talkTexts = { "What a fine day to be living... I mean be dead...", "What? Me, a reaper? No... I'm not... Tell anyone and your dead..."};
    //public string[][] shoptenderMainTexts = { barMainTexts, gunMainTexts, theifMainTexts, tailorMainTexts };

    public GameObject[] amountButtons;
    private int itemSelected = -1;
    private int amountBuying;
    public GameObject buyButton;
    public TextMeshProUGUI shopBuyButtonText;
    public TextMeshProUGUI returnButtonText;
    public Sprite[] buttonSprites;

    public void StartShop(Shoptender newShoptender)
    {
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraRatio>().AddShopCanvas(GetComponent<CanvasScaler>());
        shoptender = newShoptender;
        //shopType = shoptender.shopType;
        //shoptenderImage.sprite = shoptenderSprites[(int)shopType];
        items = shoptender.items;
        itemPrices = shoptender.itemPrices;
        itemAmount = shoptender.itemAmount;
        //itemPriceTypes = shoptender.itemPriceTypes;
        //shopName.text = shopNames[(int)shopType];
        //shopMainText.text = shoptenderMainTexts[(int)shopType][Random.Range(0, shoptenderMainTexts[(int)shopType].Length)];
        for (int i = 0; i < buttons.Length; i++)
        {
            itemImages[i].sprite = itemTypeSprites[(int)items[i]];
            if (items[i] == ItemType.Empty)
            {
                buttons[i].interactable = false;
                buyButtons[i].SetActive(false);
                soldOutButtons[i].SetActive(true);
            }
            //else if (itemPriceTypes[i] == MoneyType.Money)
            //{//Normally images are money sprite
            buttonPriceTexts[i].text = "$" + ((int)(itemPrices[i] / 100)) + "." + (itemPrices[i] % 100 == 0 ? "00" : itemPrices[i] / 10 % 10 == 0 ? "0" + (itemPrices[i] % 100).ToString() : (itemPrices[i] % 100).ToString());
            /*}
            else if (itemPriceTypes[i] == MoneyType.chemicals)
            {
                buttonMoneyImages[i * 2].sprite = moneyImageType[1];
                buttonMoneyImages[i * 2 + 1].sprite = moneyImageType[1];
                buttonPriceTexts[i].text = shoptender.itemPrices[i].ToString();
            }*/
        }
    }

    public void BuyItem(int buttonNum)
    {
        //Inventory Add statement
        //if (shoptender.gameShop)
        //{
        //PlayerBehaviour player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        //if (player.money >= itemPrices[buttonNum])
        //{
        if (itemSelected == -1)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
            itemSelected = buttonNum;
            amountBuying = 1;
            buyButton.SetActive(true);
            shopBuyButtonText.text = items[itemSelected] + "[" + amountBuying + "] $" + itemPrices[itemSelected];
            returnButtonText.text = "Cancel";
            //buttons[itemSelected].GetComponent<Image>().sprite = ;
            amountButtons[itemSelected * 2].SetActive(true);
            amountButtons[itemSelected * 2+1].SetActive(true);
            buttonPriceTexts[itemSelected].text = "X" + amountBuying;
        }
        else
        {
            SlowText("Sorry... You're too poor... I don't speak to peasents...");
        }
        //}
        //else
        //{
            /*if (PlayerPrefs.GetInt(itemPriceTypes[buttonNum].ToString()) >= itemPrices[buttonNum])
            {
                PlayerPrefs.SetInt(itemPriceTypes[buttonNum].ToString(), PlayerPrefs.GetInt(itemPriceTypes[buttonNum].ToString()) - itemPrices[buttonNum]);
                if (items[buttonNum] == ItemType.flower)
                {
                    PlayerPrefs.SetFloat("Stamina", PlayerPrefs.GetFloat("Stamina")+10f);
                }
                buyAudio.Play();
                buttons[buttonNum].interactable = false;
                itemImages[buttonNum].sprite = itemTypeSprites[0];//Empty
                buyButtons[buttonNum].SetActive(false);
                soldOutButtons[buttonNum].SetActive(true);
                items[buttonNum] = ItemType.Empty;
                //GameObject.FindGameObjectWithTag("TownUI").GetComponent<TownUI>().UpdateValues();
            }*/
        //}
    }

    public void MainBuyButton()
    {
        //if (items[buttonNum] == ItemType.flower)
        //{
            //GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            //added = gc.AddInventory(ItemType.Molotov);
        //}
        buyAudio.Play();
        //player.money -= itemPrices[buttonNum];
        //player.moneyText.text = "$" + ((int)(player.money / 100)) + "." + (player.money % 100 == 0 ? "00" : player.money / 10 % 10 == 0 ? "0" + (player.money % 100).ToString() : (player.money % 100).ToString());
        itemAmount[itemSelected] -= amountBuying;
        amountButtons[itemSelected * 2].SetActive(false);
        amountButtons[itemSelected * 2 + 1].SetActive(false);
        if (itemAmount[itemSelected] == 0)
        {
            itemImages[itemSelected].sprite = itemTypeSprites[0];//Empty
            items[itemSelected] = ItemType.Empty;
            buyButtons[itemSelected].SetActive(false);
            soldOutButtons[itemSelected].SetActive(true);
        }
        else
        {
            buttonPriceTexts[itemSelected].text = "$" + ((int)(itemPrices[itemSelected] / 100)) + "." + (itemPrices[itemSelected] % 100 == 0 ? "00" : itemPrices[itemSelected] / 10 % 10 == 0 ? "0" + (itemPrices[itemSelected] % 100).ToString() : (itemPrices[itemSelected] % 100).ToString());
            buyButtons[itemSelected].SetActive(true);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            if (items[i] != ItemType.Empty)
            {
                buttons[i].interactable = true;
            }
        }
        itemSelected = -1;
        returnButtonText.text = "Return";
        buyButton.SetActive(false);
    }

    public void Add()
    {
        if (amountBuying < itemAmount[itemSelected] || itemAmount[itemSelected] <= -1)
        {
            amountBuying++;
            buttonPriceTexts[itemSelected].text = "X" + amountBuying;
            shopBuyButtonText.text = items[itemSelected] + "[" + amountBuying + "] $" + (itemPrices[itemSelected] * amountBuying);
        }
    }

    public void Subtract()
    {
        if (amountBuying > 1)
        {
            amountBuying--;
            buttonPriceTexts[itemSelected].text = "X" + amountBuying;
            shopBuyButtonText.text = items[itemSelected] + "[" + amountBuying + "] $" + (itemPrices[itemSelected] * amountBuying);
        }
    }

    public void ExitShop()
    {
        if (itemSelected == -1)
        {
            shoptender.items = items;
            shoptender.itemPrices = itemPrices;
            shoptender.CloseShop();
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (items[i] != ItemType.Empty)
                {
                    buttons[i].interactable = true;
                }
            }
            buttonPriceTexts[itemSelected].text = "$" + ((int)(itemPrices[itemSelected] / 100)) + "." + (itemPrices[itemSelected] % 100 == 0 ? "00" : itemPrices[itemSelected] / 10 % 10 == 0 ? "0" + (itemPrices[itemSelected] % 100).ToString() : (itemPrices[itemSelected] % 100).ToString());
            returnButtonText.text = "Return";
            amountButtons[itemSelected * 2].SetActive(false);
            amountButtons[itemSelected * 2 + 1].SetActive(false);
            buyButtons[itemSelected].SetActive(true);
            itemSelected = -1;
            buyButton.SetActive(false);
        }
    }

    public void SlowText(string newText)
    {
        if (textCoroutine !=null)
        {
            StopCoroutine(textCoroutine);
        }
        textCoroutine = StartCoroutine(SlowTextCoroutine(newText));
    }

    IEnumerator SlowTextCoroutine(string newText)
    {
        shopMainText.text = "";
        for (int i = 0; i < newText.Length; i++)
        {
            //yield return new WaitForSecondsRealtime(textSpeeds[(int)shopType]);
            yield return new WaitForSecondsRealtime(.02f);
            shopMainText.text += newText[i];
        }
    }
}
