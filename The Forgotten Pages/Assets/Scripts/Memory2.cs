using Cinemachine;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Build.Content;
using UnityEngine;

/* 
 * Anna Breuker
 * Memory2.cs
 * Project 5
 * the cutscene details for memory 2
 */

public class Memory2 : MonoBehaviour, Memory
{
    public MemoryNPC motherCharacter;
    public MemoryTutorialBox tutorialBox;
    public MemoryExitDoor exitDoor;

    public GameObject player;
    public Vector3 playerStartPos;

    public PolygonCollider2D memoryConfiner;
    public CinemachineVirtualCamera cmCamera;

    public PlayerMovement player3D;

    // Start is called before the first frame update
    void Start()
    {
        //StartCutscene();
        player3D = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
        cmCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = memoryConfiner;
        player.transform.position = playerStartPos;
    }

    public IEnumerator Cutscene()
    {
        yield return new WaitUntil(() => GameController.instance.isInMemory);

        player.GetComponent<MemoryPlayerMovement>().canMove = false;
        yield return new WaitForSeconds(1);

        motherCharacter.StartDialouge(new string[] {"Okay honey, now it's your turn.", "Try and come up with a spooky story!"});
        //Debug.Log("waiting for mom to stop talking");
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        //Debug.Log("should be setting flashlight to active");
        player.GetComponent<MemoryPlayerMovement>().flashlight.SetActive(true);
        yield return new WaitForSeconds(.5f);

        motherCharacter.StartDialouge(new string[] { "Don't forget to flip on the flashlight!" });
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        tutorialBox.description = "Flip on the flashlight by pressing [F]";
        tutorialBox.gameObject.SetActive(true);
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
        yield return new WaitUntil(() => player.GetComponent<MemoryPlayerMovement>().flashlightOn);

        tutorialBox.gameObject.SetActive(false);
        motherCharacter.StartDialouge(new string[] { "...", ". . .", "Ah, it's alright.", "You always did hate being put on the spot." });
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        player.GetComponent<MemoryPlayerMovement>().flashlight.SetActive(false);
        motherCharacter.StartDialouge(new string[] {"I think your Dad's waiting for us back by the tent.", "We can bring him some smores as a treat!"});
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 115f;
        motherCharacter.speed = 3;

        player.GetComponent<MemoryPlayerMovement>().canMove = true;

        yield return new WaitUntil(() => GameController.instance.isInMemory == false);

        player3D.playerUpgrades = new PlayerFlashlight(player3D.playerUpgrades, player3D.flashlight);
        GameController.instance.memoriesCollected++;
        GameController.instance.memoryImages[1].color = Color.white;
        if (GameController.instance.memoriesCollected == 4)
        {
            GameController.instance.memoryImages[4].color = Color.white;
        }

    }

    public void RecordMemory()
    {
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
    }
}

