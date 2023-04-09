using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using TMPro;

public class MemoryNPC : MonoBehaviour
{
    public string[] dialouge;
    public GameObject textbox;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        StartDialouge(new string[] {"This is the placeholder text!", "woo!", "woooooo!" });
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPosition(-3, 3);
    }

    public void MoveToPosition(float newPos, float speed)
    {
        if (gameObject.transform.position.x < newPos - .1f || gameObject.transform.position.x > newPos + .1f)
        {
            if (gameObject.transform.position.x < newPos)
            {
                gameObject.transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
            }
            else if (gameObject.transform.position.x > newPos)
            {
                gameObject.transform.position -= new Vector3(Time.deltaTime * speed, 0, 0);
            }
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
    }

    //TODO: method for movement, method for talking. 
    //      should make a controller to be able to script these cutscenes better.
    //      could use a design pattern to get this done.
}
