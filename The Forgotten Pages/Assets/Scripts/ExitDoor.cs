using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public GameObject winScreen;

    public GameObject lockedText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.instance.memoriesCollected >= 3)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.Confined;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().jumpscareImage.gameObject.SetActive(false);
                winScreen.SetActive(true);
                Debug.Log("player wins!");
            }
            else
            {
                lockedText.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        lockedText.SetActive(false);
    }
}
