using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Memory1 : MonoBehaviour
{
    public MemoryNPC motherCharacter;
    public MemoryNPC importantKidCharacter;
    public MemoryNPC[] otherChildren;
    public GameObject player;

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
    }

    public IEnumerator Cutscene()
    {
        yield return new WaitUntil(() => GameController.instance.isInMemory);
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
        importantKidCharacter.newPos = 110;
        importantKidCharacter.speed = 4;
        yield return new WaitUntil(() => importantKidCharacter.moveToNewPos == false);

        //prompt player to sprint.
        //wait until player sprints
        
        //let player move
        //wait until player hits trigger to fall (possibly put animation here)

        //mom character runs over and is saying something
        //wait until motherCharacter.moveToNewPos == false

        //dialouge
        //a lot of motherCharacter.isTalking == false
        
        //let the player move around, once they leave the screen or hit a door they're back to the horror game.
    }
}
