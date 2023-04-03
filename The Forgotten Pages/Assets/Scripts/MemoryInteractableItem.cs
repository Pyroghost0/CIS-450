using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class MemoryInteractableItem : MonoBehaviour
{
    public GameObject textbox;
    public TextMeshProUGUI text;
    public string description;
    public bool isPressed;
    public bool inRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = description;
        if (inRange )
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isPressed)
            {
                textbox.SetActive(true);
                isPressed = true;
            }
            else if(Input.GetKeyDown(KeyCode.Space) && isPressed)
            {
                textbox.SetActive(false);
                isPressed = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("2DPlayer"))
        {
            inRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("2DPlayer"))
        {
            inRange = false;
        }
    }
}
