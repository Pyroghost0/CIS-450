using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMonsterSight : MonoBehaviour
{
    public Transform tunnelMonsterTransform;
    public TunnelMonster tunnelMonster;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
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
