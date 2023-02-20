using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float levelLength;
    private float timeRemaining;
    public ProgressBar moonlightBar;
    //public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        moonlightBar = GetComponent<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining += Time.deltaTime;
        if (timeRemaining <= levelLength)
        {
            moonlightBar.current = ((levelLength - timeRemaining) / levelLength)*100;
        }
        else
        { 
            moonlightBar.current = 0;
        }
    }
}
