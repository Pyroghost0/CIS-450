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
using System.Buffers;

public class GameController : Singleton<GameController>
{
    public static GameController instance;

    public Transform player;
    public Vector3 spawnPos;
    public Spawner spawner;
    public TextMeshProUGUI librarianText;
    public Image libraianOutline;
    public List<Transform> navGraph = new List<Transform>();
    public List<List<Transform>> tunnelGraph = new List<List<Transform>>();
    public List<Transform> mimicPositions;
    public List<Transform> ragdollPositions;

    public bool isInMemory;
    public GameObject memoryManger;
    public Memory[] memories = new Memory[] {new Memory1(), new Memory2(), new Memory3(), new Memory4(), new Memory5() };
    public GameObject finalMemory;
    public int memoriesCollected;
    public Image[] memoryImages;
    public GameObject gameUI;
    //public TextMeshProUGUI memoryText;

    public bool isPaused;
    public GameObject pauseScreen;
    public GameObject tutorialPannel;
    public TextMeshProUGUI mainTutorialText;
    public TextMeshProUGUI descriptionTutorialText;
    public List<Transform> tutorialNavGraph;
    private string[] tutorialMainTexts = { "The Library", "Movement", "The Librarian", "Memory Fragments" };
    private string[] tutorialDescriptionTexts = { "You are a child trapped in a library. \nYou don't remember how you got here. \nYou don't remember who you are. \nYou do not know what lurks in the shadows. \nYou need to remember.",
        "To move, press the WASD or arrow keys.", "The Librarian is near you, and can hear your movements. Walking will make more noise, but you can hide from her if you get out of her path and stop moving.", 
        "These books are fragments of your memeory. Collect all 5 memory fragments to be able to escape." };

    public AudioSource heartBeat;
    public AudioSource breathing;

    public int tutorialNum;


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
        memories = new Memory[] {memoryManger.gameObject.GetComponent<Memory1>(), memoryManger.gameObject.GetComponent<Memory2>(), memoryManger.gameObject.GetComponent<Memory3>(), memoryManger.gameObject.GetComponent<Memory4>(), memoryManger.gameObject.GetComponent<Memory5>() };

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
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !player.GetComponent<PlayerMovement>().gameOverScreen.activeSelf)
        {
            Pause();
        }
        /*if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchGameMode(4);
        }*/
        //memoryText.text = "Memories Collected:\n" + memoriesCollected + "/3";
    }

    public void SwitchGameMode(int memoryNumber)
    {
        isInMemory = !isInMemory;
        if (isInMemory)
        {
            //pause everything in the 3D scene
            spawnPos = player.transform.position;
            memories[memoryNumber].StartCutscene();
        }
        if (!isInMemory)
        {
            //play everything in the 3D scene
        }
        gameUI.SetActive(!isInMemory);
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
            libraianOutline.color = new Color(1f, 1f, 1f, timer * 2f);
            //libraianOutline.color = new Color(0f, 0f, 0f, timer / .5f);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer = .5f;
        color.a = 1f;
        librarianText.color = color;
        libraianOutline.color = new Color(1f, 1f, 1f, timer * 2f);
        //libraianOutline.color = new Color(0f, 0f, 0f, 0f);
        yield return new WaitForSeconds(3f);
        while (timer > 0f)
        {
            color.a = timer / .5f;
            librarianText.color = color;
            libraianOutline.color = new Color(1f, 1f, 1f, timer * 2f);
            //libraianOutline.color = new Color(0f, 0f, 0f, timer / .5f);
            timer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        color.a = 0f;
        librarianText.color = color;
        libraianOutline.color = new Color(1f, 1f, 1f, 0f);
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
        int mimicSpawn1 = -1;
        int mimicSpawn2 = -1;
        bool activeTunnel = false;
        float timeSpawnedShy = 0f;
        bool shyActive = false;
        int ragSpawn1 = -1;
        int ragSpawn2 = -1;
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        while (true)
        {
            if (!isInMemory && !playerMovement.frozenOverlay.gameObject.activeSelf)
            {
                if (librarianActive)
                {
                    librarianActive = false;
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
                if (shyActive)
                {
                    shyActive = false;
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.GetComponent<Enemy>().enemyType == EnemyType.ShyMonster)
                        {
                            shyActive = true;
                            timeSpawnedShy = Time.timeSinceLevelLoad + 25f;
                            break;
                        }
                    }
                }
                int spawnNum = Random.Range(0, memoriesCollected >= 3 ? 4 : memoriesCollected+1);

                //Librarian
                if (spawnNum == 0 && !librarianActive && Time.timeSinceLevelLoad >= timeSpawnedLibrarian)
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

                //Mimic
                else if (spawnNum == 1)
                {
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    bool foundFirst = false;
                    bool foundSecond = false;
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.GetComponent<Enemy>().enemyType == EnemyType.Mimic)
                        {
                            if ((mimicSpawn1 != -1 && mimicSpawn2 == -1) || foundSecond || (enemy.transform.position - mimicPositions[mimicSpawn1].position).magnitude < (enemy.transform.position - mimicPositions[mimicSpawn2].position).magnitude)
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
                        //Debug.Log(mimicSpawn1 + "\t\t" + mimicSpawn2);
                        List<Transform> closestTransforms = new List<Transform>();
                        List<int> positionNumbers = new List<int>();
                        for (int i = 0; i < mimicPositions.Count; i++)
                        {
                            if (i != mimicSpawn1 && i != mimicSpawn2)
                            {
                                int insertInto = closestTransforms.Count;
                                for (int j = 0; j < closestTransforms.Count; j++)
                                {
                                    if ((player.position - mimicPositions[i].position).magnitude < (player.position - closestTransforms[j].position).magnitude)
                                    {
                                        insertInto = j;
                                        break;
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
                        //Debug.Log(mimicSpawn1 + "\t\t" + mimicSpawn2);
                    }
                }

                //Tunnel
                /*else if (spawnNum == 2)
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
                }*/

                //Shy *not Among Us* monster
                else if (spawnNum == 2 && !shyActive && Time.timeSinceLevelLoad >= timeSpawnedShy)
                {
                    List<Transform> closestTransforms = new List<Transform>();
                    for (int i = 0; i < navGraph.Count; i++)
                    {
                        int insertInto = closestTransforms.Count;
                        for (int j = 0; j < closestTransforms.Count; j++)
                        {
                            if ((player.position - navGraph[i].position).magnitude < (player.position - closestTransforms[j].position).magnitude)
                            {
                                insertInto = j;
                                break;
                            }
                        }
                        closestTransforms.Insert(insertInto, navGraph[i]);
                    }
                    spawner.SpawnEnemy(closestTransforms[2].position, EnemyType.ShyMonster);
                    shyActive = true;
                    Debug.Log("Spawn Shy");
                }

                //Ragdoll
                else if (spawnNum == 3)
                {
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    bool foundFirst = false;
                    bool foundSecond = false;
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.GetComponent<Enemy>().enemyType == EnemyType.RagdollMonster)
                        {
                            if ((ragSpawn1 != -1 && ragSpawn2 == -1) || foundSecond || (enemy.transform.position - ragdollPositions[ragSpawn1].position).magnitude < (enemy.transform.position - ragdollPositions[ragSpawn2].position).magnitude)
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
                        ragSpawn1 = foundFirst ? ragSpawn1 : -1;
                        ragSpawn2 = foundSecond ? ragSpawn2 : -1;
                        //Debug.Log(mimicSpawn1 + "\t\t" + mimicSpawn2);
                        List<Transform> closestTransforms = new List<Transform>();
                        List<int> positionNumbers = new List<int>();
                        for (int i = 0; i < ragdollPositions.Count; i++)
                        {
                            if (i != ragSpawn1 && i != ragSpawn2)
                            {
                                int insertInto = closestTransforms.Count;
                                for (int j = 0; j < closestTransforms.Count; j++)
                                {
                                    if ((player.position - ragdollPositions[i].position).magnitude < (player.position - closestTransforms[j].position).magnitude)
                                    {
                                        insertInto = j;
                                        break;
                                    }
                                }
                                closestTransforms.Insert(insertInto, ragdollPositions[i]);
                                positionNumbers.Insert(insertInto, i);
                            }
                        }
                        spawner.SpawnEnemy(closestTransforms[2].position, EnemyType.RagdollMonster);
                        if (!foundFirst)
                        {
                            ragSpawn1 = positionNumbers[2];
                        }
                        else
                        {
                            ragSpawn2 = positionNumbers[2];
                        }
                        Debug.Log(ragSpawn1 + "\tRagdoll\t" + ragSpawn2);
                    }
                }
            }
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void SetUpTutorialPannel(int num)
    {
        tutorialNum = num;
        if (num == 2)
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
        mainTutorialText.text = tutorialMainTexts[2];
        descriptionTutorialText.text = tutorialDescriptionTexts[2];
    }

    public void Pause()
    {
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);
        Cursor.lockState = tutorialPannel.activeSelf || isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = isPaused || tutorialPannel.activeSelf ? 0f : 1f;
    }

    public void Continue()
    {
        if (tutorialNum == 0)
        {
            SetUpTutorialPannel(1);
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            tutorialPannel.SetActive(false);
        }
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        TunnelMonsterSight.moreHidden = false;
        Destroy(gameObject);//Perissting across games causes referance issues like button UI
        SceneManager.LoadScene("Game");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        player.transform.position = spawnPos;
        player.gameObject.GetComponent<PlayerMovement>().gameOverScreen.SetActive(false);
        player.gameObject.GetComponent<PlayerMovement>().sanityBar.fillAmount = 1;
        Cursor.lockState = CursorLockMode.Locked;

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
