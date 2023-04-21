using Cinemachine;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/* 
 * Anna Breuker
 * Memory3.cs
 * Project 5
 * the cutscene details for memory 3
 */

public class Memory3 : MonoBehaviour, Memory
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

        motherCharacter.StartDialouge(new string[] { "This is where the new dialouge will go", "for this fluff cutscene i haven't written yet!" });
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        player.GetComponent<MemoryPlayerMovement>().canMove = true;
        GameController.instance.memoriesCollected++;
        Debug.Log(GameController.instance.memoriesCollected);

    }

    public void RecordMemory()
    {
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
    }
}
