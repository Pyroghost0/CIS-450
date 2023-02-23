/* Caleb Kahn
 * GameController
 * Project 1
 * Controls some aspects of the game like pausing
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public string sceneName;
    public GameObject winPannel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
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
