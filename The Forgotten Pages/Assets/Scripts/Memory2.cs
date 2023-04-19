using Cinemachine;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Build.Content;
using UnityEngine;

public class Memory2 : MonoBehaviour, Memory
{
    public MemoryNPC motherCharacter;
    public MemoryTutorialBox tutorialBox;
    public MemoryExitDoor exitDoor;

    public GameObject player;
    public Vector3 playerStartPos;

    public PolygonCollider2D memoryConfiner;
    public CinemachineVirtualCamera cmCamera;

    // Start is called before the first frame update
    void Start()
    {
        StartCutscene();
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
        Debug.Log("waiting for mom to stop talking");
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        Debug.Log("should be setting flashlight to active");
        player.GetComponent<MemoryPlayerMovement>().flashlight.SetActive(true);
        yield return new WaitForSeconds(.5f);

        motherCharacter.StartDialouge(new string[] { "Don't forget to flip on the flashlight!" });
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        tutorialBox.description = "Flip on the flashlight by pressing [F]";
        tutorialBox.gameObject.SetActive(true);
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
        yield return new WaitUntil(() => player.GetComponent<MemoryPlayerMovement>().flashlightOn);

        tutorialBox.gameObject.SetActive(false);
        motherCharacter.StartDialouge(new string[] { "..." });
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        player.GetComponent<MemoryPlayerMovement>().canMove = true;

    }

    public void RecordMemory()
    {
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
    }
}

