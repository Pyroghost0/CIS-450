using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour, ISubject
{
    public GameObject pauseMenu;
    public List<IObserver> observers = new List<IObserver>();
    public bool canTimeScale = true;
    public bool canPause = true;
    public bool isPaused = false;

    public void NotifyObservers(bool paused)
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateSelf(paused);
        }
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Continue()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        if (canTimeScale)
        {
            Time.timeScale = 1f;
        }
        NotifyObservers(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            if (canTimeScale)
            {
                Time.timeScale = isPaused ? 0f : 1f;
            }
            NotifyObservers(isPaused);
        }
    }
}
