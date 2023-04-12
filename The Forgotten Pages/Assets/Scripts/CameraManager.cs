using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject memoryCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.SetActive(true);
        memoryCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isInMemory)
        {
            mainCamera.SetActive(false);
            memoryCamera.SetActive(true);
        }
        else
        {
            mainCamera.SetActive(true);
            memoryCamera.SetActive(false);
        }
    }
}
