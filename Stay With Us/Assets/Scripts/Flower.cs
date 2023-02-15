using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public Sprite[] stagesOfGrowth;
    public SpriteRenderer spriteRenderer;
    public int stageOfGrowth = 0;
    public float growthStageTime = 20;
    public float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        spriteRenderer.sprite = stagesOfGrowth[stageOfGrowth];
        if (timer > growthStageTime && stageOfGrowth < stagesOfGrowth.Length)
        { 
            stageOfGrowth++;
            timer = 0;
        }
    }
}
