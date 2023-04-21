using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using TMPro;

/*
 * Anna Breuker
 * MemoryNPC.cs
 * Project 5
 * Code that contains the methods needed to be accessed by the memory cutscenes for each npc.
 */
public class MemoryNPC : MonoBehaviour
{
    public string[] dialouge;
    public GameObject textbox;
    public TextMeshProUGUI text;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public GameObject[] emotes;

    public bool moveToNewPos;
    public float newPos;
    public float speed;

    public bool isTalking;
    // Start is called before the first frame update
    void Start()
    {
        //StartDialouge(new string[] {"This is the placeholder text!", "woo!", "woooooo!" });
        spriteRenderer= GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToNewPos)
        {
            Debug.Log("Moving to new position");
            MoveToPosition(newPos, speed);
        }
    }

    public IEnumerator PlayAnimation(string animationVariable, float playTime)
    { 
        animator.SetBool(animationVariable, true);
        yield return new WaitForSeconds(playTime);
        animator.SetBool(animationVariable, false);
    }

    public void MoveToPosition(float newPos, float speed)
    {
        Debug.Log("inside move to position");
        if (gameObject.transform.position.x < newPos - .1f || gameObject.transform.position.x > newPos + .1f)
        {
            animator.SetBool("isMoving", true);
            if (gameObject.transform.position.x < newPos)
            {
                gameObject.transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
                spriteRenderer.flipX = false;
            }
            else if (gameObject.transform.position.x > newPos)
            {
                gameObject.transform.position -= new Vector3(Time.deltaTime * speed, 0, 0);
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            moveToNewPos= false;
        }
    }

    public void MoveToRandomPosition(float maxPos, float minPos, float speed)
    { 
        
    }

    public void StartDialouge(string[] newDialouge)
    { 
        dialouge = newDialouge;
        StartCoroutine(Talk());
    }

    public IEnumerator Talk()
    {
        isTalking= true;
        textbox.SetActive(true);
        text.text = dialouge[0];
        for (int i = 1; i < dialouge.Length; i++)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            Debug.Log(i);
            text.text = dialouge[i];
            yield return new WaitForSeconds(.5f);
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        textbox.SetActive(false);
        isTalking= false;
    }

    public IEnumerator Emote(GameObject emote, float waitTime)
    { 
        emote.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        emote.SetActive(false);
    }
}
