using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoryTutorialBox : MonoBehaviour
{
    public GameObject textbox;
    public TextMeshProUGUI text;
    public string description;
    public bool isPressed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = description;
    }
}
