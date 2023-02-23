using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotOfDirt : MonoBehaviour
{
    public GameObject[] flowers; //could use a design pattern for this. strategy?
                                 //or decorator for the stages of growth.

    public Grave grave;
    public bool flowerPlanted;

    

    /*public PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }*/

    //takes the number of flower to plant from the inventory
    //then shows the sprite and will manage other variables.
    public void PlantFlower(int flowerNum)
    {
        flowers[flowerNum].SetActive(true);
        flowers[flowerNum].GetComponent<Flower>().stageOfGrowth = 0;
        
        for(int i = 0; i < grave.flowerPreferences.Length; i++) 
        {
            if (grave.flowerPreferences[i] == flowers[flowerNum].GetComponent<Flower>().flowerType)
            {
                Debug.Log("grave loves this");
                Destroy(grave.GetComponent<GraveReaction>());
                grave.reaction = grave.gameObject.AddComponent<Loved>();

                grave.flowerLovedIndicator.SetActive(true);
                grave.flowerHatedIndicator.SetActive(false);
            }
            else if (grave.flowerHates[i] == flowers[flowerNum].GetComponent<Flower>().flowerType)
            {
                Debug.Log("grave hates this");
                Destroy(grave.GetComponent<GraveReaction>());
                grave.reaction = grave.gameObject.AddComponent<Hated>();

                grave.flowerLovedIndicator.SetActive(false);
                grave.flowerHatedIndicator.SetActive(true);
            }
            else
            {
                Destroy(grave.GetComponent<GraveReaction>());
                grave.reaction = grave.gameObject.AddComponent<Neutral>();

                grave.flowerLovedIndicator.SetActive(false);
                grave.flowerHatedIndicator.SetActive(false);
            }
        }
        if (grave.rememberance >= 100 - grave.reaction.UpdateRemembrance())
        {
            if (grave.reaction.UpdateRemembrance() > 0)
            {
                grave.rememberance = 100;
            }
            else
            {
                grave.rememberance -= 10;
            }
        }
        else if (grave.rememberance < 10 && grave.reaction.UpdateRemembrance() < 0)
        {
            grave.rememberance = 0;
        }
        else
        {
            grave.rememberance += grave.reaction.UpdateRemembrance();
        }

    }
}
