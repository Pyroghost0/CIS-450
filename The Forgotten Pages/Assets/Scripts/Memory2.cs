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
        //StartCutscene();
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
        motherCharacter.StartDialouge(new string[] {"Okay honey, now it's your turn.", "Try and come up with a spooky story!", });

        yield return new WaitUntil(() => motherCharacter.isTalking = false);
        player.GetComponent<MemoryPlayerMovement>().flashlight.SetActive(true);

        motherCharacter.StartDialouge(new string[] { "Don't forget to flip on the flashlight!" });
        yield return new WaitUntil(() => motherCharacter.isTalking = false);

        tutorialBox.GetComponent<TextMeshProUGUI>().text = "Flip on the flashlight by pressing [F]";
        tutorialBox.gameObject.SetActive(true);
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;

        yield return new WaitUntil(() => player.GetComponent<MemoryPlayerMovement>().flashlightOn);

        motherCharacter.StartDialouge(new string[] { "..." });
        yield return new WaitUntil(() => motherCharacter.isTalking = false);
        player.GetComponent<MemoryPlayerMovement>().canMove = true;

    }

    public void RecordMemory()
    {
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
    }
}

