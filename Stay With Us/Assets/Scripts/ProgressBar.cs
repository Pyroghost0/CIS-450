using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Anna Breuker
 * ProgressBar.cs
 * Project 2
 * Manages the progress bars on the graves and day to night.
 */
public class ProgressBar : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image mask;
    //public Image fill;
    //public Color color;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        mask.fillAmount = fillAmount;

        //fill.color = color;
    }
}
