/* Caleb Kahn
 * MainMenuManager
 * Project 1
 * UI for main menu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public GameObject inputObstructor;
    public Button[] levelButtons;
    public RectTransform mainMenu;

    /* To load main menu from levels use this *Change last function from "Tutorial" to scene name (can keep scene name as parametor manually changed in each scene on the object)
    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        AsyncOperation ao1 = SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive);
        yield return new WaitUntil(() => ao1.isDone);
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuManager>().InstantLoad();
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Tutorial"));
    }*/

    public void InstantLoad()
    {
        mainMenu.anchoredPosition = new Vector2(-384f, 0);
    }

    public void MoveRight()
    {
        StartCoroutine(MoveScreenRight());
    }

    IEnumerator MoveScreenRight()
    {
        inputObstructor.SetActive(true);
        float timer = 0;
        while (timer < 1.8f)//Main menu is 1.8 screens wide
        {
            //mainMenu.anchoredPosition -= new Vector2(426.6f * Time.deltaTime, 0);//960 is screen width
            mainMenu.anchoredPosition = new Vector2(384f * Mathf.Cos(Mathf.PI * timer / 1.8f), 0);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        mainMenu.anchoredPosition = new Vector2(-384f, 0);
        inputObstructor.SetActive(false);
    }

    public void MoveLeft()
    {
        StartCoroutine(MoveScreenLeft());
    }

    IEnumerator MoveScreenLeft()
    {
        inputObstructor.SetActive(true);
        float timer = 0;
        while (timer < 1.8f)//Main menu is 1.8 screens wide
        {
            //mainMenu.anchoredPosition += new Vector2(426.6f * Time.deltaTime, 0);//960 is screen width
            mainMenu.anchoredPosition = new Vector2(-384f * Mathf.Cos(Mathf.PI * timer / 1.8f), 0);
            //mapScreen.anchoredPosition += new Vector2(mainMenu.rect.x * Time.deltaTime, 0);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        mainMenu.anchoredPosition = new Vector2(384f, 0);
        //mapScreen.anchoredPosition = Vector2.zero;
        //inMap = true;
        inputObstructor.SetActive(false);
    }

    public void LoadLevel(int level)
    {
        if (level == 0)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (level == 1)
        {
            SceneManager.LoadScene("Prototype 1");
        }
        else if (level == 2)
        {
            SceneManager.LoadScene("Prototype 2");
        }
        else if (level == 3)
        {
            SceneManager.LoadScene("Prototype 3");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
