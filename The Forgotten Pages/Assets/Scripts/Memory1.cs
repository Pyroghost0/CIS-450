using Cinemachine;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

/*
 * Anna Breuker
 * Memory1.cs
 * Project 5
 * the cutscene details for memory 1 
 */

public class Memory1 : MonoBehaviour, Memory
{
    public MemoryNPC motherCharacter;
    public MemoryNPC importantKidCharacter;
    public MemoryNPC[] otherChildren;
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
        //player shouldn't be able to move yet.
        Debug.Log("cutscene started");
        importantKidCharacter.moveToNewPos = true;
        importantKidCharacter.newPos = 100.3f;
        importantKidCharacter.speed = 2;

        yield return new WaitUntil(() => importantKidCharacter.moveToNewPos == false);
        //Debug.Log("huh i didn't think that would work but if you're seeing this it did!");

        importantKidCharacter.StartDialouge(new string[] {"Tag! You're it!"});
        yield return new WaitUntil(() => importantKidCharacter.isTalking == false);

        importantKidCharacter.moveToNewPos = true;
        importantKidCharacter.newPos = 115;
        importantKidCharacter.speed = 4;

        yield return new WaitForSeconds(.25f);

        //prompt player to sprint.
        tutorialBox.description = "Use [A] and [D] or the arrow keys to move.";
        tutorialBox.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.2f);

        tutorialBox.description = "You better catch up with them! Press [SPACE] or [SHIFT] to sprint!";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        player.GetComponent<MemoryPlayerMovement>().canMove = true;
        player.GetComponent<MemoryPlayerMovement>().sprintUnlocked = true;
        //wait until player sprints
        yield return new WaitUntil(() => player.GetComponent<MemoryPlayerMovement>().isSprinting);
        
        //deactivate prompt
        tutorialBox.gameObject.SetActive(false);

        //wait until player hits trigger to fall (possibly put animation here)
        yield return new WaitUntil(() => player.transform.position.x > 105);
       // player.GetComponent<MemoryPlayerMovement>().canMove = false;
        player.GetComponent<MemoryPlayerMovement>().Fall();
        
        //mom character runs over and is saying something
        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 104.5f;
        motherCharacter.speed = 4;


        //wait until motherCharacter.moveToNewPos == false
        yield return new WaitUntil(() => motherCharacter.moveToNewPos == false);

        //dialouge
        motherCharacter.StartDialouge(new string[] {"Oh dear, that was quite the fall!", "Shhh, shhh. Don't cry! I've got just the thing.", "It's nothing a band-aid can't fix."});
        //a lot of motherCharacter.isTalking == false
        yield return new WaitUntil(() => motherCharacter.isTalking == false);

        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 105f;
        motherCharacter.speed = 4;
        
        yield return new WaitUntil(() => motherCharacter.moveToNewPos == false);
        player.GetComponent<MemoryPlayerMovement>().GetBackUp();

        motherCharacter.StartDialouge(new string[] { "See? There you go. All better.", "You want to go home?", "We can pick up some icecream on the way!" });

        yield return new WaitUntil(() => motherCharacter.isTalking == false);

        motherCharacter.moveToNewPos = true;
        motherCharacter.newPos = 115f;
        motherCharacter.speed = 3;

        yield return new WaitUntil(() => motherCharacter.moveToNewPos == false);
        //let the player move around, once they leave the screen or hit a door they're back to the horror game.
        player3D.playerUpgrades = new PlayerSprint(player3D.playerUpgrades, player3D);
        GameController.instance.memoriesCollected++;

    }

    public void RecordMemory()
    {
        player.GetComponent<MemoryPlayerMovement>().sprintUnlocked = true;
    }
}
