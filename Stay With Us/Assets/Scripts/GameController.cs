/* Caleb Kahn
 * GameController
 * Project 1
 * Controls some aspects of the game like pausing
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public string sceneName;
    public GameObject winPannel;
    public TextMeshProUGUI winResultText;
    public TextMeshProUGUI winDescriptionText;
    private string[] winTexts = { "They don't hate you, so I think you've won", "Bet you wouldn't have won if the flowers needed sunlight to grow", "Did you win? Maybe you can rest in peace now", "Insert funny HaHa joke here so you feel accomplished", 
    "This wouldn't have been possible with the Discord mod ghost", "Winning? More like a slave to society, am I right?", "Now just ask death for Portal 3", "You know, a lot of people would die to be where you are now"};
    private string[] loseTexts = { "Congradulations, you've successfully failed at life twice", "If you've faied at life once, you can fail again", "Who said you only have one chance at life, try again", "After all that work they hate you, maybe they deserve not to be remembered", 
    "Nothing in life came easy, it probably the same in death", "I mean you're trading their ectoplasm, so it makes sence why they're mad at you", "When life gave you lemons, you forgot to ask how much ectoplasm they're worth", "Losing is anouther way of saying you lose"};
    public float levelLength = 240f;
    public float timeRemaining;
    public ProgressBar moonlightBar;

    public ItemType[] summonableObjectTypes = { ItemType.SunflowerSeed, ItemType.MagnoliaSeed, ItemType.IrisSeed , ItemType.PoppySeed , ItemType.RoseSeed };
    private LayerMask layerMask;
    public Collider2D graveyardCollider;
    public Collider2D[] dontStopColliderSpots = new Collider2D[0];
    //public EdgeCollider2D mapCollider;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Graveyard");
        StartCoroutine(LevelTimer());
        StartCoroutine(RandomItemSummon());
        //Debug.Log(RanddomSpawnPosition());
    }

    IEnumerator LevelTimer()
    {
        while (timeRemaining < levelLength)
        {
            moonlightBar.current = ((levelLength - timeRemaining) / levelLength) * 100f;
            yield return new WaitForFixedUpdate();
            timeRemaining += Time.deltaTime;
        }
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
        while (timeRemaining < levelLength)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            if (Random.value <.3f)
            {
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(ItemType.Ectoplasm, RanddomSpawnPosition(), true);
            }
            else
            {
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(summonableObjectTypes[Random.Range(0, summonableObjectTypes.Length)], RanddomSpawnPosition(), true);
            }
        }
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
        return new Vector2(Random.Range(rays[0].point.x, rays[1].point.x), Random.Range(rays[0].point.y, rays[1].point.y));
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
