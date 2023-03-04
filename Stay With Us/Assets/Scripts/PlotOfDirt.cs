/* Anna Breuker
 * PlotOfDirt
 * Project 1
 * Plants flowers for a grave
 */
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
    public bool newFlowerCanBePlanted = true;

    public void Start()
    {
        newFlowerCanBePlanted = true;
    }
    //takes the number of flower to plant from the inventory
    //then shows the sprite and will manage other variables.
    public void PlantFlower(int flowerNum)
    {
        if (newFlowerCanBePlanted)
        {
            bool flowerPlanted = false;
            for (int i = 0; i < flowers.Length; i++)
            {
                flowers[i].SetActive(false);
            }
            flowers[flowerNum].SetActive(true);
            flowers[flowerNum].GetComponent<Flower>().stageOfGrowth = 0;

            Debug.Log("flowerNum is " + flowerNum);

            for (int i = 0; i < grave.flowerPreferences.Length; i++)
            {
                Debug.Log(grave.flowerPreferences[i]);
                if (grave.flowerHates.Length > i)
                {
                    Debug.Log(grave.flowerPreferences[i]);
                }
                Debug.Log(flowers[flowerNum]);
                if (grave.flowerPreferences[i] == flowers[flowerNum].GetComponent<Flower>().flowerType && !flowerPlanted)
                {
                    Debug.Log("grave loves this");
                    Destroy(grave.GetComponent<GraveReaction>());
                    grave.reaction = grave.gameObject.AddComponent<Loved>();

                    grave.flowerLovedIndicator.SetActive(true);
                    grave.flowerHatedIndicator.SetActive(false);

                    flowerPlanted = true;
                }
            }
            for (int i = 0; i < grave.flowerHates.Length; i++)
            {
                if (grave.flowerHates.Length > i && grave.flowerHates[i] == flowers[flowerNum].GetComponent<Flower>().flowerType && !flowerPlanted)
                {
                    Debug.Log("grave hates this");
                    Destroy(grave.GetComponent<GraveReaction>());
                    grave.reaction = grave.gameObject.AddComponent<Hated>();

                    grave.flowerLovedIndicator.SetActive(false);
                    grave.flowerHatedIndicator.SetActive(true);

                    flowerPlanted = true;
                }
            }
            if (!flowerPlanted)
            {
                Destroy(grave.GetComponent<GraveReaction>());
                grave.reaction = grave.gameObject.AddComponent<Neutral>();

                grave.flowerLovedIndicator.SetActive(false);
                grave.flowerHatedIndicator.SetActive(false);

                flowerPlanted = true;
            }


            grave.rememberanceRate = grave.reaction.UpdateRemembrance();
            newFlowerCanBePlanted = false;
        }
        

    }

    public void FlowerDied()
    {
        grave.rememberanceRate = .5f;
        newFlowerCanBePlanted = true;
    }

}
