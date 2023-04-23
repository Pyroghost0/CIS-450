/* Caleb Kahn, Anna Breuker, Cooper Denault
 * GameController
 * Project 5
 * General functions such as spawning enemies and winning the game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : Singleton<GameController>
{
    public static GameController instance;

    public Transform player;
    public Spawner spawner;
    public TextMeshProUGUI librarianText;
    public Image libraianOutline;
    public List<Transform> navGraph = new List<Transform>();
    public List<List<Transform>> tunnelGraph = new List<List<Transform>>();
    public List<Transform> mimicPositions;

    public bool isInMemory;
    public GameObject memoryManger;
    public Memory[] memories = new Memory[] {new Memory1(), new Memory2() };
    public int memoriesCollected;
    public TextMeshProUGUI memoryText;

    public GameObject tutorialPannel;
    public TextMeshProUGUI mainTutorialText;
    public TextMeshProUGUI descriptionTutorialText;
    public List<Transform> tutorialNavGraph;
    private string[] tutorialMainTexts = { "Movement", "The Librarian", "Memory Fragments" };
    private string[] tutorialDescriptionTexts = { "To move, press the WASD or arrow keys.", "The Librarian is near you, and can hear your movements. Walking will make more noise, but you can hide from her if you get out of her path and stop moving.", 
        "These books are fragments of your memeory. Collect all 5 memory fragments to be able to escape. NOTE only 2 fragments are in the game demo." };

    public AudioSource heartBeat;
    public AudioSource breathing;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            //make sure this persists across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("Trying to instantiate a second" +
                "instance of singleton GameManager");
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!tutorialPannel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isInMemory = false;
        memories = new Memory[] {memoryManger.gameObject.GetComponent<Memory1>(), memoryManger.gameObject.GetComponent<Memory2>(), memoryManger.gameObject.GetComponent<Memory3>() };

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            navGraph.Add(transform.GetChild(0).GetChild(i));
        }
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            List<Transform> tunnels = new List<Transform>();
            for (int j = 0; j < transform.GetChild(1).GetChild(i).childCount; j++)
            {
                tunnels.Add(transform.GetChild(1).GetChild(i).GetChild(j));
            }
            tunnelGraph.Add(tunnels);
        }
        SetUpTutorialPannel(0);
        //SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchGameMode(0);
        }
        memoryText.text = "Memories Collected:\n" + memoriesCollected + "/3";
    }

    public void SwitchGameMode(int memoryNumber)
    {
        isInMemory = !isInMemory;
        if (isInMemory)
        {
            //pause everything in the 3D scene
            memories[memoryNumber].StartCutscene();
        }
        if (!isInMemory)
        {
            //play everything in the 3D scene
        }
    }

    public void LibrarianInArea()
    {
    
        StartCoroutine(LibrarianInAreaCoroutine());


    }

    IEnumerator LibrarianInAreaCoroutine()
    {
        heartBeat.Play();
        breathing.Play();


        float timer = 0;
        Color color = librarianText.color;
        while (timer < .5f)
        {
            
            color.a = timer / .5f;
            librarianText.color = color;
            libraianOutline.color = new Color(.5f, .5f, .5f, timer);
            //libraianOutline.color = new Color(0f, 0f, 0f, timer / .5f);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer = .5f;
        color.a = 1f;
        librarianText.color = color;
        libraianOutline.color = new Color(.5f, .5f, .5f, .5f);
        //libraianOutline.color = new Color(0f, 0f, 0f, 0f);
        yield return new WaitForSeconds(3f);
        while (timer > 0f)
        {
            color.a = timer / .5f;
            librarianText.color = color;
            libraianOutline.color = new Color(0f, 0f, 0f, timer);
            //libraianOutline.color = new Color(0f, 0f, 0f, timer / .5f);
            timer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        color.a = 0f;
        librarianText.color = color;
        libraianOutline.color = new Color(.5f, .5f, .5f, 0f);
        //libraianOutline.color = new Color(0f, 0f, 0f, 0f);

        heartBeat.Stop();
        breathing.Stop();

    }

    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        float timeSpawnedLibrarian = 0f;
        bool librarianActive = false;
        bool activeTunnel = false;
        int mimicSpawn1 = -1;
        int mimicSpawn2 = -1;
        while (true)
        {
            yield return new WaitUntil(() => !isInMemory);
            if (librarianActive)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Enemy>().enemyType == EnemyType.Librarian)
                    {
                        librarianActive = true;
                        timeSpawnedLibrarian = Time.timeSinceLevelLoad + 15f;
                        break;
                    }
                }
            }
            int spawnNum = Time.timeSinceLevelLoad >= timeSpawnedLibrarian ? Random.Range(0, 3) * 2 : Random.Range(1, 3) * 2;
            if (spawnNum == 0)
            {
                Position closestPosition = navGraph[0].GetComponent<Position>();
                for (int i = 1; i < navGraph.Count; i++)
                {
                    if ((transform.position - closestPosition.transform.position).magnitude > (transform.position - navGraph[i].position).magnitude)
                    {
                        closestPosition = navGraph[i].GetComponent<Position>();
                    }
                }
                Transform farthest = closestPosition.otherPositions[0];
                for (int i = 1; i < closestPosition.otherPositions.Count; i++)
                {
                    if ((player.position - farthest.transform.position).magnitude < (player.position - closestPosition.otherPositions[i].position).magnitude)
                    {
                        farthest = closestPosition.otherPositions[i];
                    }
                }
                librarianActive = true;
                spawner.SpawnEnemy(farthest.position, EnemyType.Librarian);
            }
            else if (spawnNum == 2)
            {
                activeTunnel = false;
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Enemy>().enemyType == EnemyType.TunnelMonster)
                    {
                        activeTunnel = true;
                        //break;
                    }
                }
                if (!activeTunnel)
                {
                    Transform closest = tunnelGraph[0][0];
                    foreach (List<Transform> paths in tunnelGraph)
                    {
                        foreach (Transform pathPosition in paths)
                        {
                            if ((player.position - pathPosition.position).magnitude < (player.position - closest.position).magnitude)
                            {
                                closest = pathPosition;
                            }
                        }
                    }
                    spawner.SpawnEnemy(closest.parent.GetChild(0).GetChild(0).position, EnemyType.TunnelMonster);
                }
            }
            else// if (spawnNum == 4)
            {
                activeTunnel = false;
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                bool foundFirst = false;
                bool foundSecond = false;
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Enemy>().enemyType == EnemyType.Mimic)
                    {
                        if ((mimicSpawn1 != -1 && mimicSpawn2 == -1) ||  foundSecond || (enemy.transform.position - mimicPositions[mimicSpawn1].position).magnitude < (enemy.transform.position - mimicPositions[mimicSpawn2].position).magnitude)
                        {
                            foundFirst = true;
                        }
                        else
                        {
                            foundSecond = true;
                        }
                        if (foundFirst && foundSecond)
                        {
                            break;
                        }
                    }
                }
                if (!foundFirst || !foundSecond)
                {
                    mimicSpawn1 = foundFirst ? mimicSpawn1 : -1;
                    mimicSpawn2 = foundSecond ? mimicSpawn2 : -1;
                    Debug.Log(mimicSpawn1 + "\t\t" + mimicSpawn2);
                    List<Transform> closestTransforms = new List<Transform>();
                    List<int> positionNumbers = new List<int>();
                    for (int i = 0; i < mimicPositions.Count; i++)
                    {
                        if (i != mimicSpawn1 && i != mimicSpawn2)
                        {
                            int insertInto = 0;
                            for (int j = 0; j < closestTransforms.Count; j++)
                            {
                                if ((player.position - mimicPositions[i].position).magnitude < (player.position - closestTransforms[j].position).magnitude)
                                {
                                    insertInto = j;
                                }
                            }
                            closestTransforms.Insert(insertInto, mimicPositions[i]);
                            positionNumbers.Insert(insertInto, i);
                        }
                    }
                    spawner.SpawnEnemy(closestTransforms[2].position, EnemyType.Mimic);
                    if (!foundFirst)
                    {
                        mimicSpawn1 = positionNumbers[2];
                    }
                    else
                    {
                        mimicSpawn2 = positionNumbers[2];
                    }
                    Debug.Log(mimicSpawn1 + "\t\t" + mimicSpawn2);
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void SetUpTutorialPannel(int num)
    {
        if (num == 1)
        {
            StartCoroutine(DelayForLibrarian());
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            tutorialPannel.SetActive(true);
            mainTutorialText.text = tutorialMainTexts[num];
            descriptionTutorialText.text = tutorialDescriptionTexts[num];
        }
    }

    IEnumerator DelayForLibrarian()
    {
        spawner.SpawnEnemy(tutorialNavGraph[0].position, EnemyType.Librarian);
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<Librarian>().navGraph = tutorialNavGraph;
        yield return new WaitForSeconds(2.25f);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        tutorialPannel.SetActive(true);
        mainTutorialText.text = tutorialMainTexts[1];
        descriptionTutorialText.text = tutorialDescriptionTexts[1];
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        tutorialPannel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);//Perissting across games causes referance issues like button UI
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);//Perissting across games causes referance issues like button UI
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Debug.Log("Quitting Game");
        Application.Quit();

    }
}
