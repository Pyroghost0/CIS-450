/* Anna Breuker
 * Flower
 * Project 1
 * Flower that slowly grows over time
 */
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
    public PlotOfDirt plotOfDirt;

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
            if (stageOfGrowth >= stagesOfGrowth.Length -1)
            {
                spriteRenderer.sprite = stagesOfGrowth[stagesOfGrowth.Length - 1];
                //gameObject.SetActive(false);
                plotOfDirt.FlowerDied();
                Debug.Log("flowerdied");
            }
            else
            {
                spriteRenderer.sprite = stagesOfGrowth[stageOfGrowth];
                timer = 0;
            }
        }
    }
}
