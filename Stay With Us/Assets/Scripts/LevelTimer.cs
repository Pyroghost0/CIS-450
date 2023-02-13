using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float levelLength;
    private float timer;
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
        timer += Time.deltaTime;
        if (timer <= levelLength)
        {
            moonlightBar.current = ((levelLength - timer) / levelLength)*100;
        }
        else
        { 
            moonlightBar.current = 0;
        }
    }
}
