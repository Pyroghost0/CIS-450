using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Memory4 : MonoBehaviour, Memory
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
    yield return new WaitForSeconds(.1f);

    motherCharacter.moveToNewPos = true;
    motherCharacter.newPos = 97.91f;
    motherCharacter.speed = 2;

    motherCharacter.StartDialouge(new string[] { "Oh, so here's where you ran off to!", "Looking for Dad?", "He told me he'd meet us back at home.", "Come on honey, let's go, the sun's almost set.", "We can pick up food on the way home." });
    yield return new WaitUntil(() => !motherCharacter.isTalking);

    motherCharacter.moveToNewPos = true;
    motherCharacter.newPos = 104.5f;
    motherCharacter.speed = 2;

    player.GetComponent<MemoryPlayerMovement>().canMove = true;

    yield return new WaitUntil(() => GameController.instance.isInMemory == false);


    GameController.instance.memoriesCollected++;
    GameController.instance.memoryImages[3].color = Color.white;
    if (GameController.instance.memoriesCollected == 4)
    {
        GameController.instance.memoryImages[4].color = Color.white;
    }
    Debug.Log(GameController.instance.memoriesCollected);

}

public void RecordMemory()
{
    player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
}
}

