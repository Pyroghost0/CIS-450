using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public Sprite[] stagesOfGrowth;
    public SpriteRenderer spriteRenderer;
    public int stageOfGrowth = 0;
    private float growthStageTime = 10f;
    public float timer;

    public string flowerType; // could also be enum
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(transform.position.y * -10);
        spriteRenderer.sprite = stagesOfGrowth[stageOfGrowth];
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > growthStageTime && (stageOfGrowth != stagesOfGrowth.Length -1 || timer > growthStageTime * 2f))// && stageOfGrowth < stagesOfGrowth.Length - 1)
        { 
            stageOfGrowth++;
            if (stageOfGrowth == stagesOfGrowth.Length)
            {
                gameObject.SetActive(false);
            }
            else
            {
                spriteRenderer.sprite = stagesOfGrowth[stageOfGrowth];
                timer = 0;
            }
        }
    }
}
