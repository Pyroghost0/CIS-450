using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory5 : MonoBehaviour, Memory
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
        player.GetComponent<SpriteRenderer>().flipX = false;
    }

    public IEnumerator Cutscene()
    {
        yield return new WaitUntil(() => GameController.instance.isInMemory);

        player.GetComponent<MemoryPlayerMovement>().canMove = false;
        yield return new WaitForSeconds(.1f);

        tutorialBox.description = "You're playing hide and seek! Press [E] to hide.";
        tutorialBox.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        tutorialBox.gameObject.SetActive(false);

        player.GetComponent<SpriteRenderer>().sortingOrder = -1;

        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 99;
        motherCharacter.speed = 2;
        yield return new WaitUntil(() => motherCharacter.moveToNewPos == false);

        motherCharacter.StartDialouge(new string[] { "Ready or not, here I come!"});
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 101;
        motherCharacter.speed = 1;
        yield return new WaitUntil(() => motherCharacter.moveToNewPos == false);

        motherCharacter.StartDialouge(new string[] { "Hmmmmm, not here..." });
        yield return new WaitUntil(() => !motherCharacter.isTalking);


        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 94;
        motherCharacter.speed = 1;
        yield return new WaitUntil(() => motherCharacter.moveToNewPos == false);

        motherCharacter.StartDialouge(new string[] { "Huh, well I'm stumped!", "Guess I'll just have to go to the park all by myself." });
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        player.GetComponent<SpriteRenderer>().sortingOrder = 10;

        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 95;
        motherCharacter.speed = 2;
        motherCharacter.StartDialouge(new string[] { "Oh! There you are!", "Haha, ready to go? I think Dad said he'd meet us there." });
        yield return new WaitUntil(() => !motherCharacter.isTalking);

        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 106;
        motherCharacter.speed = 2;

        player.GetComponent<MemoryPlayerMovement>().canMove = true;

        yield return new WaitUntil(() => GameController.instance.isInMemory == false);

        //need to fix this at the meeting
        GameController.instance.memoriesCollected++;
        GameController.instance.memoryImages[3].color = Color.white;
        if (GameController.instance.memoriesCollected == 3)
        {
            GameController.instance.finalMemory.SetActive(true);
        }
        //Debug.Log(GameController.instance.memoriesCollected);

    }

    public void RecordMemory()
    {
        player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
    }
}

