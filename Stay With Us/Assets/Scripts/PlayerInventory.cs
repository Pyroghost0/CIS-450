/* Caleb Kahn
 * PlayerInventory
 * Project 1
 * Stores the items obtained
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour, IObserver
{
    public ItemType[] inventory;
    public ItemType item
    {
        get
        {
            return inventory[inventorySelection];
        }
    }
    public string[] itemTexts = { "", "?", "Daisy Seed", "Sunflower Seed", "Forget-Me-Not Seed", "Poppy Seed", "Rose Seed", "Sands Of Time" };
    public int[] inventoryAmount;
    public int inventorySize = 6;
    public Sprite[] itemSprites;
    public int inventorySelection = 0;
    public Image[] invFrames;
    public Image[] invItems;
    public TextMeshProUGUI[] amountTexts;
    private float scrollAmount = 0f;

    public int money;
    public TextMeshProUGUI moneyText;
    public bool isPaused = false;

    public TextMeshProUGUI itemNameText;

    /* Use Items like this
     * if (Input.GetMouseButtonDown(1) && !gc.isPaused && canUseItems)
        {
            if (playerInventory.item == ItemType.Molotov)
            {
                StartCoroutine(ThrowMolotov());
                playerInventory.UseItem();
            }
    */

    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = money.ToString();
        //inventorySize = PlayerPrefs.GetInt("Inventory");
        inventory = new ItemType[inventorySize];
        inventoryAmount = new int[inventorySize];
        for (int i = 0; i < invFrames.Length; i++)
        {
            if (i < inventorySize)
            {
                if (i == 0)
                {
                    invFrames[i].color = Color.white;
                }
                else
                {
                    invFrames[i].color = new Color(.75f, .75f, .75f, 1f);
                }
            }
            else
            {
                invFrames[i].gameObject.SetActive(false);
            }
        }
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>().RegisterObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)// && Time.timeScale != 0f)//Pause checker
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                scrollAmount += Input.GetAxis("Mouse ScrollWheel");
                //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
                if (scrollAmount > .5f)
                {
                    invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                    inventorySelection++;
                    if (inventorySelection == inventorySize)
                    {
                        inventorySelection = 0;
                    }
                    invFrames[inventorySelection].color = Color.white;
                    scrollAmount = 0f;
                }
                else if (scrollAmount < -.5f)
                {
                    invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                    inventorySelection--;
                    if (inventorySelection == -1)
                    {
                        inventorySelection = inventorySize - 1;
                    }
                    invFrames[inventorySelection].color = Color.white;
                    scrollAmount = 0f;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))// && inventorySize > 0)
            {
                invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                inventorySelection = 0;
                invFrames[inventorySelection].color = Color.white;
                scrollAmount = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))// && inventorySize > 1)
            {
                invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                inventorySelection = 1;
                invFrames[inventorySelection].color = Color.white;
                scrollAmount = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))// && inventorySize > 2)
            {
                invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                inventorySelection = 2;
                invFrames[inventorySelection].color = Color.white;
                scrollAmount = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))// && inventorySize > 3)
            {
                invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                inventorySelection = 3;
                invFrames[inventorySelection].color = Color.white;
                scrollAmount = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))// && inventorySize > 4)
            {
                invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                inventorySelection = 4;
                invFrames[inventorySelection].color = Color.white;
                scrollAmount = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))// && inventorySize > 5)
            {
                invFrames[inventorySelection].color = new Color(.75f, .75f, .75f, 1f);
                inventorySelection = 5;
                invFrames[inventorySelection].color = Color.white;
                scrollAmount = 0f;
            }
        }
        itemNameText.text = itemTexts[(int)inventory[inventorySelection]];
    }

    public void UpdateMoney(int difference)
    {
        money += difference;
        moneyText.text = money.ToString();
    }

    public bool AddInventory(ItemType item, int amount = 1)//Returns true if added
    {
        int firstEmpty = -1;
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i] == ItemType.Empty && firstEmpty == -1)
            {
                firstEmpty = i;
            }
            else if (inventory[i] == item)
            {
                inventoryAmount[i] += amount;
                //amountTexts[i].gameObject.SetActive(true);
                amountTexts[i].text = "x" + inventoryAmount[i];
                return true;
            }
        }
        if (firstEmpty != -1)
        {
            inventory[firstEmpty] = item;
            invItems[firstEmpty].sprite = itemSprites[(int)item];
            inventoryAmount[firstEmpty] = amount;
            //if (amount > 1)
            //{
                amountTexts[firstEmpty].gameObject.SetActive(true);
                amountTexts[firstEmpty].text = "x" + inventoryAmount[firstEmpty];
            //}
            return true;
        }
        return false;
    }

    public void UseItem()
    {
        inventoryAmount[inventorySelection]--;
        if (inventoryAmount[inventorySelection] == 0)
        {
            inventory[inventorySelection] = ItemType.Empty;
            invItems[inventorySelection].sprite = itemSprites[0];//Empty sprite
        //}
        //else if (inventoryAmount[inventorySelection] == 1)
        //{
            amountTexts[inventorySelection].gameObject.SetActive(false);
        }
        else
        {
            amountTexts[inventorySelection].text = "x" + inventoryAmount[inventorySelection];
        }
    }

    public bool CanAdd(ItemType itemType)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == itemType || inventory[i] == ItemType.Empty)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateSelf(bool paused)
    {
        isPaused = paused;
    }
}
