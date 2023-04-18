/* Caleb Kahn
 * TutorialTriggerEvent
 * Project 5
 * If collided sets text in game controller
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerEvent : MonoBehaviour
{
    public int triggerNum = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.instance.SetUpTutorialPannel(triggerNum);
            Destroy(gameObject);
        }
    }
}
