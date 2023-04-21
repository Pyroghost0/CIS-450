/* Caleb Kahn, Anna Breuker
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

    public Spawner spawner;
    public TextMeshProUGUI librarianText;
    public Image libraianOutline;
    public List<Transform> navGraph = new List<Transform>();
    public List<List<Transform>> tunnelGraph = new List<List<Transform>>();

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
    private string[] tutorialDescriptionTexts = { "To move, press the WASD or arrow keys.", "The Librarian is near you, and can hear your movements. Walking will make more noice, but you can hide from her if you stop moving and get out of her path.", 
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
        //StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    SwitchGameMode(2);
        //}
        memoryText.text = "Memories Collected:\n" + memoriesCollected + "/2";
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

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        //spawner.SpawnEnemy(new Vector3(0f, 2f, 16f), EnemyType.Librarian);
        spawner.SpawnEnemy(tunnelGraph[0][0].position, EnemyType.TunnelMonster);
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
        Destroy(gameObject);//Perissting across games causes referance issues like button UI
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Debug.Log("Quitting Game");
        Application.Quit();

    }
}
