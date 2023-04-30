/* Caleb Kahn
 * TunnelMonsterSight
 * Project 5
 * The sight mechanism for the tunnel monster
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMonsterSight : MonoBehaviour
{
    public Transform tunnelMonsterTransform;
    public TunnelMonster tunnelMonster;
    public static bool moreHidden = false;

    void Start()
    {
        if (moreHidden)
        {
            transform.localScale = new Vector3(10f, 4f, 22.5f);
            transform.position = new Vector3(0f, 0f, 8.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !tunnelMonster.frozen)
        {
            RaycastHit rayHit;
            //Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
            if (Physics.Raycast(tunnelMonsterTransform.position, other.transform.position - tunnelMonsterTransform.position, out rayHit))
            {
                //Debug.Log(rayHit.collider.name + "\n" + rayHit.point);
                if (rayHit.collider.CompareTag("Player"))
                {
                    //Debug.Log("Sees you");
                    tunnelMonster.seesPlayer = true;
                }
            }
        }
    }
}
