using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotOfDirt : MonoBehaviour
{
    public GameObject[] flowers; //could use a design pattern for this. strategy?
                                 //or decorator for the stages of growth.

    public Grave grave;
    public bool flowerPlanted;

    public PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //takes the number of flower to plant from the inventory
    //then shows the sprite and will manage other variables.
    public void PlantFlower(int flowerNum)
    {
        flowers[flowerNum].SetActive(true);
        flowers[flowerNum].GetComponent<Flower>().stageOfGrowth = 0;
        //instead of hardcoding this, i'd like the remembernce to be tied to flower type and grave preferences. will fix once prototype works (and design patterns implemented)
        if (grave.rememberance >= 80)
        {
            grave.rememberance = 100;
        }
        else
        {
            grave.rememberance += 20;
        }
        //can manage other flower related variables here. 
    }
}
