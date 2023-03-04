/* Caleb Kahn, Anna Breuker
 * GameController
 * Project 1
 * Controls some aspects of the game like game timer and random spawning
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DigitalRuby.SimpleLUT;

public class GameController : MonoBehaviour
{
    public string sceneName;
    public GameObject winPannel;
    public TextMeshProUGUI winResultText;
    public TextMeshProUGUI winDescriptionText;
    private string[] winTexts = { "They don't hate you, so I think you've won", "Bet you wouldn't have won if the flowers needed sunlight to grow", "Did you win? Maybe you can rest in peace now", "Insert funny HaHa joke here so you feel accomplished",
    "This wouldn't have been possible with the Discord mod ghost", "Winning? More like a slave to society, am I right?", "Now just ask death for Portal 3", "You know, a lot of people would die to be where you are now"};
    private string[] loseTexts = { "Congratulations, you've successfully failed at life twice", "If you've failed at life once, you can fail again", "Who said you only have one chance at life, try again", "After all that work they hate you, maybe they deserve not to be remembered",
    "Nothing in life came easy, it's probably the same in death", "I mean you're trading their ectoplasm, so it makes sense why they're mad at you", "When life gave you lemons, you forgot to ask how much ectoplasm they're worth", "Losing is another way of saying you lose"};
    public float levelLength = 240f;
    public float timeRemaining;
    public ProgressBar moonlightBar;

    public ItemType[] summonableObjectTypes = { ItemType.DaisySeed, ItemType.SunflowerSeed, ItemType.ForgetMeNotSeed, ItemType.PoppySeed, ItemType.RoseSeed };
    private LayerMask layerMask;
    public Collider2D graveyardCollider;
    public Collider2D[] dontStopColliderSpots = new Collider2D[0];

    public bool isTutorial;

    public GameObject player;
    public PlayerInventory playerInventory;
    public GameObject inivisWall;
    public GameObject inivisWallCurrency;
    public GameObject invisShopWall;
    public GameObject inivisWallSeeds;
    public GameObject lorePanel;
    public GameObject tutorialPanel;
    public TextMeshProUGUI loreText;
    public TextMeshProUGUI tutorialText;
    public bool textRead;

    public GameObject poppySeed;
    public GameObject sunflowerSeed;
    public Grave tutorialGrave;
    public SimpleLUT cameraLUT;
    //public EdgeCollider2D mapCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        layerMask = LayerMask.GetMask("Graveyard");
        if (!isTutorial)
        {
            StartCoroutine(LevelTimer());
            StartCoroutine(RandomItemSummon());
            cameraLUT.Brightness = -0.25f;
            cameraLUT.Saturation = -0.25f;
        }
        if (isTutorial)
        {
            StartCoroutine(Tutorial());
        }
        //Debug.Log(RanddomSpawnPosition());
    }

    IEnumerator LevelTimer()
    {
        while (timeRemaining < levelLength)
        {
            moonlightBar.current = ((levelLength - timeRemaining) / levelLength) * 100f;
            yield return new WaitForFixedUpdate();
            timeRemaining += Time.deltaTime;
            if (cameraLUT.Brightness <= 0)
			{
                cameraLUT.Brightness += 0.2f * Time.deltaTime / levelLength;
                cameraLUT.Saturation += 0.75f * Time.deltaTime / levelLength;
            }
        }
        gameObject.GetComponent<Pauser>().canPause = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().isPaused = true;
        moonlightBar.current = 0f;
        Time.timeScale = 0f;
        GameObject[] graveObjects = GameObject.FindGameObjectsWithTag("Grave");
        float averageLikenessLevel = 0f;
        for (int i = 0; i < graveObjects.Length; i++)
        {
            averageLikenessLevel += graveObjects[i].GetComponent<Grave>().rememberance;
        }
        averageLikenessLevel /= graveObjects.Length;
        Debug.Log(averageLikenessLevel);
        if (averageLikenessLevel >= 50f)
        {
            winResultText.text = "You Win";//Even though it is that for default
            winDescriptionText.text = winTexts[Random.Range(0, winTexts.Length)];
        }
        else
        {
            winResultText.text = "You Lose";
            winDescriptionText.text = loseTexts[Random.Range(0, loseTexts.Length)];
        }
        winPannel.SetActive(true);
    }

    IEnumerator RandomItemSummon()
    {
        for (float i = 30f; i > 0f; i-= Random.Range(1f, 3f))
        {
            if (Random.value < .3f)
            {
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(ItemType.Ectoplasm, RanddomSpawnPosition(), true, i);
            }
            else
            {
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(summonableObjectTypes[Random.Range(0, summonableObjectTypes.Length)], RanddomSpawnPosition(), true, i);
            }
        }
        while (timeRemaining < levelLength)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            if (Random.value < .3f)
            {
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(ItemType.Ectoplasm, RanddomSpawnPosition(), true);
            }
            else
            {
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(summonableObjectTypes[Random.Range(0, summonableObjectTypes.Length)], RanddomSpawnPosition(), true);
            }
        }
    }

    public void ContinueButton()
    {
        textRead = true;
    }

    IEnumerator Tutorial()
    {
        //LORE
        loreText.text = "Unfortunately, you have died." +
            "\nSurprisingly, your death is of use!" +
            "\n\nDeath has tasked you with overseeing the graves of those who have also passed." +
            "\n\nThey deserve happiness. You will give it to them.";
        
        yield return new WaitUntil(() => textRead);
        lorePanel.SetActive(false);
        //Move
        tutorialPanel.SetActive(true);
        tutorialText.text = "To move, use WASD or arrow keys.";
        yield return new WaitUntil(() => (player.transform.position.x > -7));
        tutorialText.text = "Great job! You can use SPACE or SHIFT to sprint.";
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        inivisWall.SetActive(false);
        tutorialText.text = "You can pick up ectoplasm to use as currency in the shop.\nYou can see how much you have in the top right.";
        yield return new WaitUntil(() => (playerInventory.money > 5));
        inivisWallCurrency.SetActive(false);

        //Shop tutorial
        tutorialText.text = "Now lets look at the shop. Walk over to the grim reaper to buy and sell seeds & special items";
        yield return new WaitUntil(() => !textRead);
        invisShopWall.SetActive(false);
        GameObject.FindGameObjectWithTag("Shoptender").GetComponent<Shoptender>().enabled = false;

        tutorialText.text = "You can pick up flower seeds and plant them to keep ghosts happy.";
        yield return new WaitUntil(() => (poppySeed == null && sunflowerSeed == null));
        tutorialText.text = "In the lower lefthand corner is your inventory. Press the number keys to change your active item.";
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Alpha2)));
        inivisWallSeeds.SetActive(false);
        tutorialText.text = "This is Elam.\nYou can talk to ghosts like them by pressing E to find their out ther interests.\nElam likes poppies. Press Q to plant seeds in front of graves.";
        yield return new WaitUntil(() => tutorialGrave.reaction != null);
        Debug.Log(tutorialGrave.reaction);
        if (tutorialGrave.reaction is Loved)
        {
            tutorialText.text = "Elam loves this flower! That made his remembrance bar go up a little bit. If the ghosts are happy they will gift you ectoplasm!\n[press ENTER to continue]";
        }
        else 
        {
            tutorialText.text = "Uh oh, Elam hates this flower. That made his remembrance bar go down. If it gets too far down the ghost will be unhappy.\n[press ENTER to continue]";
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        tutorialText.text = "The blue bar above graves indicates how happy the ghost is. At the end of the night, you want all of these to be as filled as possible.\n[press ENTER to continue]";
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        StartCoroutine(LevelTimer());
        tutorialText.text = "The 'Moonlight Remaining' bar shows you how much time is left in the night. You need to make sure to have all the ghosts happy by the end of the night.\n[press ENTER to continue]";
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        tutorialText.text = "That's it for the tutorial! Next time you step foot in the graveyard, it will be for real. Press ENTER to return to the main menu!";
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        MainMenu();
    }



    private Vector3 RanddomSpawnPosition()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        //Debug.Log(dir);
        //Debug.Log(LayerMask.GetMask("Graveyard"));
        RaycastHit2D[] rays = Physics2D.RaycastAll(Vector2.zero, dir, 50f, layerMask);
        //graveyardCollider.Cast(dir, contactFilter, rays, 50f);
        /*for (int i = 0; i < rays.Length; i++)
        {
            Debug.Log("Ray " + i + ": " + rays[i].collider.name + " at " + rays[i].point);
        }
        return Vector3.zero;*/
        return new Vector2(Random.Range(rays[rays.Length-2].point.x, rays[rays.Length-1].point.x), Random.Range(rays[0].point.y, rays[1].point.y));//No longer rays[0] and rays[1] because it can hit a ray into the 1st wall then the 1st wall again
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        Time.timeScale = 1f;
        AsyncOperation ao1 = SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive);
        yield return new WaitUntil(() => ao1.isDone);
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuManager>().InstantLoad();
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
