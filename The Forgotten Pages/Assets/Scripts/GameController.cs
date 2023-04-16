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
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchGameMode();
        }
    }

    public void SwitchGameMode()
    { 
        isInMemory= !isInMemory;
        if (isInMemory)
        {
            //pause everything in the 3D scene
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
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        //spawner.SpawnEnemy(new Vector3(0f, 2f, 16f), EnemyType.Librarian);
        spawner.SpawnEnemy(tunnelGraph[0][0].position, EnemyType.TunnelMonster);
    }
}
