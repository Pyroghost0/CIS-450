using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("cutscene started");
        importantKidCharacter.moveToNewPos = true;
        importantKidCharacter.newPos = 100.3f;
        importantKidCharacter.speed = 2;

        yield return new WaitUntil(() => importantKidCharacter.moveToNewPos == false);
        Debug.Log("huh i didn't think that would work but if you're seeing this it did!");

        importantKidCharacter.StartDialouge(new string[] {"Tag! You're it!"});
        yield return new WaitUntil(() => importantKidCharacter.isTalking == false);

        importantKidCharacter.moveToNewPos = true;
        importantKidCharacter.newPos = 110;
        importantKidCharacter.speed = 4;
        yield return new WaitUntil(() => importantKidCharacter.moveToNewPos == false);
    }
}
