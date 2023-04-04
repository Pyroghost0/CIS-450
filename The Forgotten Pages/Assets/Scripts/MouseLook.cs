/* Cooper Denault, Caleb Kahn
 * MouseLook
 * Project 5
 * Player can move mouse to look around
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public GameObject player;
    private float verticalLookRotation = 0f;

    /*private void Start()
    {
        StartCoroutine(Sight());
    }*/

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        //Get mouse input and assign to two floats
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotate player GameObject with horizontal mouse input
        player.transform.Rotate(Vector3.up * mouseX);


        //Rotate camera around the x axis with vertical mouse input
        verticalLookRotation -= mouseY;

        //Limit the vertical rotation with clamo
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        //apply rotation to our camera based on clamped output
        transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);


        //!!!!!!!!!!!!!!!!!!!!Make sure the radius in the navAgenst is less than the collider otherwise the detection is inconistant

        //Debug.Log(transform.forward);
        RaycastHit[] rayHits = Physics.RaycastAll(transform.position, transform.forward);
        /*for (int i = 0; i < rayHits.Length; i++)
        {
            Debug.Log(i + ": " + rayHits[0].collider.name + "\t\t" + rayHits[0].point);
        }
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);*/
        if (rayHits.Length > 0 && rayHits[0].collider != null)
        {
            //Debug.Log(rayHits[0].collider.name);
            //Debug.Log(rayHits[0].point);
            if (rayHits[0].collider.CompareTag("Enemy") && rayHits[0].collider.GetComponent<Enemy>().enemyType == EnemyType.ShyMonster)
            {
                Debug.Log("Was Found");
                rayHits[0].collider.GetComponent<ShyMonster>().wasFound = true;
            }
        }
    }

    IEnumerator Sight()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            //!!!!!!!!!!!!!!!!!!!!Make sure the radius in the navAgenst is less than the collider otherwise the detection is inconistant

            //Debug.Log(transform.forward);
            RaycastHit[] rayHits = Physics.RaycastAll(transform.position, transform.forward);
            for (int i = 0; i < rayHits.Length; i++)
            {
                Debug.Log(i + ": " + rayHits[0].collider.name + "\t\t" + rayHits[0].point);
            }
            Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
            if (rayHits.Length > 0 && rayHits[0].collider != null)
            {
                //Debug.Log(rayHits[0].collider.name);
                //Debug.Log(rayHits[0].point);
                if (rayHits[0].collider.CompareTag("Enemy") && rayHits[0].collider.GetComponent<Enemy>().enemyType == EnemyType.ShyMonster)
                {
                    Debug.Log("Was Found");
                    rayHits[0].collider.GetComponent<ShyMonster>().wasFound = true;
                }
            }
        }
    }
}
