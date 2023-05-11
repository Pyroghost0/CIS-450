using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/* 
 * Anna Breuker
 * Memory4.cs
 * Project 7&8
 * the cutscene details for the car memory
 */
public class Memory4 : MonoBehaviour, Memory
{
    public MemoryNPC motherCharacter;
    public MemoryNPC otherCar;
public MemoryTutorialBox tutorialBox;
public MemoryExitDoor exitDoor;

public GameObject player;
public Vector3 playerStartPos;

public PolygonCollider2D memoryConfiner;
public CinemachineVirtualCamera cmCamera;

public PlayerMovement player3D;

public ScrollingBackground road;

    public Animator waves;

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
    player.GetComponent<MemoryPlayerMovement>().canMove = false;
    player.GetComponent<SpriteRenderer>().flipX = false;
    yield return new WaitUntil(() => GameController.instance.isInMemory);

    
    yield return new WaitForSeconds(.1f);

    motherCharacter.StartDialouge(new string[] { "You excited to get to the library?", "I know, I know, you've been talking about it for weeks.", "And your favorite author's going to be there today, right?"});
    yield return new WaitUntil(() => !motherCharacter.isTalking);

    yield return new WaitForSeconds(1f);

    motherCharacter.StartDialouge(new string[] { "And the roads are so nice today!", "We should take a trip down to the park when we're done." });
    yield return new WaitUntil(() => !motherCharacter.isTalking);

    yield return new WaitForSeconds(1f);
   
    motherCharacter.StartDialouge(new string[] { "Oh, bud, you dropped something." });
    yield return new WaitUntil(() => !motherCharacter.isTalking);
    
        
    motherCharacter.GetComponent<SpriteRenderer>().flipX= true;
    yield return new WaitForSeconds(.1f);
    
    motherCharacter.StartDialouge(new string[] { "Here, let me-" });
    yield return new WaitUntil(() => !motherCharacter.isTalking);

    otherCar.moveToNewPos = true;
    otherCar.newPos = motherCharacter.gameObject.transform.position.x;
    otherCar.speed = 10;
    yield return new WaitUntil(()=> !otherCar.moveToNewPos);

    road.speed = 0f;
    waves.SetBool("isFrozen", true);
    tutorialBox.description = "Press [Q] to scream.";
    tutorialBox.gameObject.SetActive(true);
    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
    player3D.freezeSound.Play();
    player3D.frozenOverlay.gameObject.SetActive(true);
    tutorialBox.gameObject.SetActive(false);
    //could add something here with freeze effect on screen or a scream sound effect.
    yield return new WaitForSeconds(4f);
    player3D.frozenOverlay.gameObject.SetActive(false);

    GameController.instance.SwitchGameMode(0);

    yield return new WaitUntil(() => GameController.instance.isInMemory == false);


    GameController.instance.memoriesCollected++;
    GameController.instance.memoryImages[2].color = Color.white;
    GameController.instance.memoryImages[4].color = Color.white;
    player3D.playerUpgrades = new FlreezeAbility(player3D.playerUpgrades, player3D);
        player3D.speed = 2f;
        player3D.playerSprint.initSpeed = 2f;
        //Debug.Log(GameController.instance.memoriesCollected);

    }

public void RecordMemory()
{
    player.GetComponent<MemoryPlayerMovement>().flashlightUnlocked = true;
}
}

