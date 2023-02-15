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
    public GameObject[] soldOutButtons;
    private Shoptender shoptender;
    public ItemType[] items;
    public int[] itemAmount;
    private int[] itemPrices;
    private Coroutine textCoroutine;
    public AudioSource buyAudio;

    //private float[] textSpeeds = { .03f, .02f, .016f, .025f };
    //private string[] shopNames = { "The Bar", "Gun Shop", "Black Market", "Tailor" };
    public int[] itemSellPrices = { 0, 100};
    public string[] itemDescriptions = { "Sold out, read the sign...", "Some bullets..."};
    public string[] talkTexts = { "What a fine day to be living... I mean be dead...", "What? Me, a reaper? No... I'm not... Tell anyone and your dead...", "Stories? Yeah I got stories... One time someone died. The End...", "I've taken the soul out of plants before... Don't ask how...",
    "Hurry up and buy something, I might just die from boredom here...", "Yes, yes... dogs go to heaven, and cats go to hell...", "How are the plants growing so fast? Uhh... becuase I said so...", "Why do I need money? I'm just saving up so I can reincarnate into a shrimp...",
    "Are there more reapers? Yes, can't you see the one behind you?...", "Life and death are... who cares about philiosiphy anyways...", "I have plenty of hobbies killing, reaping souls... I don't have a hobby...", "THIS SCYTHE IS NOT FOR FARMING..."};
    //public string[][] shoptenderMainTexts = { barMainTexts, gunMainTexts, theifMainTexts, tailorMainTexts };

    public GameObject[] amountButtons;
    public TextMeshProUGUI[] shopItemAmountTexts;
    public int itemSelected = -1;
    private int amountBuying;
    private bool inventorySelected;
    public GameObject buyButton;
    public TextMeshProUGUI shopBuyButtonText;
    public TextMeshProUGUI returnButtonText;
    public Sprite[] buttonSprites;
    public Button[] inventoryButtons;
    public TextMeshProUGUI[] inventoryButtonPriceTexts;
    public Image[] inventoryItemImages;
    public GameObject[] inventoryAmountButtons;
    public TextMeshProUGUI[] inventoryItemAmountTexts;

    public void StartShop(Shoptender newShoptender)
    {
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraRatio>().AddShopCanvas(GetComponent<CanvasScaler>());
        shoptender = newShoptender;
        items = shoptender.items;
        itemPrices = shoptender.itemPrices;
        itemAmount = shoptender.itemAmount;
        //shopType = shoptender.shopType;
        //shoptenderImage.sprite = shoptenderSprites[(int)shopType];
        //itemPriceTypes = shoptender.itemPriceTypes;
        //shopName.text = shopNames[(int)shopType];
        //shopMainText.text = shoptenderMainTexts[(int)shopType][Random.Range(0, shoptenderMainTexts[(int)shopType].Length)];
        for (int i = 0; i < buttons.Length; i++)
        {
            itemImages[i].sprite = itemTypeSprites[(int)items[i]];
            if (items[i] == ItemType.Empty)
            {
                buttons[i].interactable = false;
                buttonPriceTexts[i].gameObject.SetActive(false);
                soldOutButtons[i].SetActive(true);
            }
            else if (itemAmount[i] != -1)
            {
                shopItemAmountTexts[i].gameObject.SetActive(true);
                shopItemAmountTexts[i].text = "x" + itemAmount[i];
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
        ItemType[] inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory;
        for (int i = 0; i < inventory.Length; i++)
        {
            inventoryItemImages[i].sprite = itemTypeSprites[(int)inventory[i]];
            if (inventory[i] == ItemType.Empty)
            {
                inventoryButtons[i].interactable = false;
                inventoryButtonPriceTexts[i].gameObject.SetActive(false);
            }
            else
            {
                inventoryItemAmountTexts[i].gameObject.SetActive(true);
                inventoryItemAmountTexts[i].text = "x" + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventoryAmount[i];
            }
            inventoryButtonPriceTexts[i].text =  "$" + ((int)(itemSellPrices[(int)inventory[i]] / 100)) + "." + (itemSellPrices[(int)inventory[i]] % 100 == 0 ? "00" : itemSellPrices[(int)inventory[i]] / 10 % 10 == 0 ? "0" + (itemSellPrices[(int)inventory[i]] % 100).ToString() : (itemSellPrices[(int)inventory[i]] % 100).ToString());
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
            if (false)
            {
                SlowText("Sorry... You're too poor... I don't speak to peasents...");
            }
            else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().CanAdd(items[buttonNum]))
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
                for (int i = 0; i < inventoryButtons.Length; i++)
                {
                    inventoryButtons[i].interactable = false;
                }
                inventorySelected = false;
                itemSelected = buttonNum;
                amountBuying = 1;
                buyButton.SetActive(true);
                shopBuyButtonText.text = items[itemSelected] + "[" + amountBuying + "] $" + itemPrices[itemSelected];
                returnButtonText.text = "Cancel";
                buttons[itemSelected].GetComponent<Image>().sprite = buttonSprites[0];
                amountButtons[itemSelected * 2].SetActive(true);
                amountButtons[itemSelected * 2 + 1].SetActive(true);
                buttonPriceTexts[itemSelected].text = "X" + amountBuying;
            }
            else
            {
                SlowText("Uhh... Look at your inventory dude...");
            }
        }
        else
        {
            
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

    public void SellItem(int buttonNum)
    {
        if (itemSelected == -1)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
            for (int i = 0; i < inventoryButtons.Length; i++)
            {
                inventoryButtons[i].interactable = false;
            }
            inventorySelected = true;
            itemSelected = buttonNum;
            amountBuying = 1;
            buyButton.SetActive(true);
            shopBuyButtonText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory[itemSelected] + "[" + amountBuying + "] $" + itemPrices[itemSelected];
            returnButtonText.text = "Cancel";
            inventoryButtons[itemSelected].GetComponent<Image>().sprite = buttonSprites[0];
            inventoryAmountButtons[itemSelected * 2].SetActive(true);
            inventoryAmountButtons[itemSelected * 2 + 1].SetActive(true);
            inventoryButtonPriceTexts[itemSelected].text = "X" + amountBuying;
        }
    }

    public void MainBuyButton()
    {
        //if (items[buttonNum] == ItemType.flower)
        //{
        //GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //added = gc.AddInventory(ItemType.Molotov);
        //}
        //buyAudio.Play();
        //player.money -= itemPrices[buttonNum];
        //player.moneyText.text = "$" + ((int)(player.money / 100)) + "." + (player.money % 100 == 0 ? "00" : player.money / 10 % 10 == 0 ? "0" + (player.money % 100).ToString() : (player.money % 100).ToString());
        if (inventorySelected)
        {
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            for (int i = 0; i < amountBuying; i++)
            {
                playerInventory.UseItem();
            }
            inventoryButtons[itemSelected].GetComponent<Image>().sprite = buttonSprites[1];
            inventoryAmountButtons[itemSelected * 2].SetActive(false);
            inventoryAmountButtons[itemSelected * 2 + 1].SetActive(false);
            inventoryButtonPriceTexts[itemSelected].text = "X" + amountBuying;
            if (playerInventory.inventoryAmount[itemSelected] == 0)
            {
                inventoryItemImages[itemSelected].sprite = itemTypeSprites[0];//Empty
                inventoryButtonPriceTexts[itemSelected].gameObject.SetActive(false);
                inventoryItemAmountTexts[itemSelected].gameObject.SetActive(false);
            }
            else
            {
                inventoryItemAmountTexts[itemSelected].text = "x" + playerInventory.inventoryAmount[itemSelected];
                inventoryButtonPriceTexts[itemSelected].text = "$" + ((int)(itemSellPrices[(int)playerInventory.inventory[itemSelected]] / 100)) + "." + (itemSellPrices[(int)playerInventory.inventory[itemSelected]] % 100 == 0 ? "00" : itemSellPrices[(int)playerInventory.inventory[itemSelected]] / 10 % 10 == 0 ? "0" + (itemSellPrices[(int)playerInventory.inventory[itemSelected]] % 100).ToString() : (itemSellPrices[(int)playerInventory.inventory[itemSelected]] % 100).ToString());
                inventoryButtonPriceTexts[itemSelected].gameObject.SetActive(true);
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().AddInventory(items[itemSelected], amountBuying);
            ItemType[] inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory;
            for (int i = 0; i < inventory.Length; i++)
            {
                inventoryItemImages[i].sprite = itemTypeSprites[(int)inventory[i]];
                if (inventory[i] == ItemType.Empty)
                {
                    inventoryButtons[i].interactable = false;
                    inventoryButtonPriceTexts[i].gameObject.SetActive(false);
                }
                else
                {
                    inventoryButtonPriceTexts[i].gameObject.SetActive(true);
                    inventoryItemAmountTexts[i].gameObject.SetActive(true);
                    inventoryItemAmountTexts[i].text = "x" + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventoryAmount[i];
                }
                inventoryButtonPriceTexts[i].text = "$" + ((int)(itemSellPrices[(int)inventory[i]] / 100)) + "." + (itemSellPrices[(int)inventory[i]] % 100 == 0 ? "00" : itemSellPrices[(int)inventory[i]] / 10 % 10 == 0 ? "0" + (itemSellPrices[(int)inventory[i]] % 100).ToString() : (itemSellPrices[(int)inventory[i]] % 100).ToString());
            }
            itemAmount[itemSelected] -= amountBuying;
            buttons[itemSelected].GetComponent<Image>().sprite = buttonSprites[1];
            amountButtons[itemSelected * 2].SetActive(false);
            amountButtons[itemSelected * 2 + 1].SetActive(false);
            if (itemAmount[itemSelected] == 0)
            {
                itemImages[itemSelected].sprite = itemTypeSprites[0];//Empty
                items[itemSelected] = ItemType.Empty;
                buttonPriceTexts[itemSelected].gameObject.SetActive(false);
                soldOutButtons[itemSelected].SetActive(true);
                shopItemAmountTexts[itemSelected].gameObject.SetActive(false);
            }
            else
            {
                shopItemAmountTexts[itemSelected].text = "x" + itemAmount[itemSelected];
                buttonPriceTexts[itemSelected].text = "$" + ((int)(itemPrices[itemSelected] / 100)) + "." + (itemPrices[itemSelected] % 100 == 0 ? "00" : itemPrices[itemSelected] / 10 % 10 == 0 ? "0" + (itemPrices[itemSelected] % 100).ToString() : (itemPrices[itemSelected] % 100).ToString());
                buttonPriceTexts[itemSelected].gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            if (items[i] != ItemType.Empty)
            {
                buttons[i].interactable = true;
            }
        }
        for (int i = 0; i < inventoryButtons.Length; i++)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory[i] != ItemType.Empty)
            {
                inventoryButtons[i].interactable = true;
            }
        }
        itemSelected = -1;
        returnButtonText.text = "Return";
        buyButton.SetActive(false);
        SlowText(talkTexts[Random.Range(0, talkTexts.Length)]);
    }

    public void Add()
    {
        if (inventorySelected)
        {
            if (amountBuying < GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventoryAmount[GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventorySelection])
            {
                amountBuying++;
                inventoryButtonPriceTexts[itemSelected].text = "X" + amountBuying;
                shopBuyButtonText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory[itemSelected] + "[" + amountBuying + "] $" + itemPrices[itemSelected];
            }
        }
        else if (amountBuying < itemAmount[itemSelected] || itemAmount[itemSelected] <= -1)
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
            if (inventorySelected)
            {
                amountBuying--;
                inventoryButtonPriceTexts[itemSelected].text = "X" + amountBuying;
                shopBuyButtonText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory[itemSelected] + "[" + amountBuying + "] $" + itemPrices[itemSelected];
            }
            else
            {
                amountBuying--;
                buttonPriceTexts[itemSelected].text = "X" + amountBuying;
                shopBuyButtonText.text = items[itemSelected] + "[" + amountBuying + "] $" + (itemPrices[itemSelected] * amountBuying);
            }
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
            for (int i = 0; i < inventoryButtons.Length; i++)
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory[i] != ItemType.Empty)
                {
                    inventoryButtons[i].interactable = true;
                }
            }
            int inventory = (int)GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory[itemSelected];
            inventoryButtonPriceTexts[itemSelected].text = "$" + ((int)(itemPrices[inventory] / 100)) + "." + (itemPrices[inventory] % 100 == 0 ? "00" : itemPrices[inventory] / 10 % 10 == 0 ? "0" + (itemPrices[inventory] % 100).ToString() : (itemPrices[inventory] % 100).ToString());
            returnButtonText.text = "Return";
            if (inventorySelected)
            {
                inventoryButtons[itemSelected].GetComponent<Image>().sprite = buttonSprites[1];
                inventoryAmountButtons[itemSelected * 2].SetActive(true);
                inventoryAmountButtons[itemSelected * 2 + 1].SetActive(true);
                inventoryButtonPriceTexts[itemSelected].text = "X" + amountBuying;
            }
            else
            {
                buttons[itemSelected].GetComponent<Image>().sprite = buttonSprites[1];
                amountButtons[itemSelected * 2].SetActive(false);
                amountButtons[itemSelected * 2 + 1].SetActive(false);
                buttonPriceTexts[itemSelected].gameObject.SetActive(true);
            }
            itemSelected = -1;
            buyButton.SetActive(false);
            SlowText(talkTexts[Random.Range(0, talkTexts.Length)]);
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
