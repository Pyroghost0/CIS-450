/* Caleb Kahn
 * SeePlayerState
 * Project 5
 * State that decides the behaivior of the tunnel enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayerState : EnemyState
{
    public TunnelMonster tunnelMonster;

    public void Start()
    {
        tunnelMonster = gameObject.GetComponent<TunnelMonster>();
    }

    public override void StartWalking()
    {
        tunnelMonster.Walk();
    }

    public override void FoundPlayer()
    {
        StartCoroutine(PlayerSpottedCoroutine());
    }

    public IEnumerator PlayerSpottedCoroutine()
    {
        while (tunnelMonster.seesPlayer)
        {
            while (tunnelMonster.seesPlayer)
            {
                Vector3 destination = tunnelMonster.player.transform.position;
                tunnelMonster.navMeshAgent.destination = destination;
                if (tunnelMonster.seesPlayer)
                {
                    tunnelMonster.seesPlayer = false;
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    yield return new WaitUntil(() => tunnelMonster.seesPlayer || Vector3.Distance(destination, transform.position) <= 1f);
                }
            }
            for (int i = 0; i < 50; i++)
            {
                if (tunnelMonster.seesPlayer)
                {
                    break;
                }
                else
                {
                    //Debug.Log("This thing is wired so don't touch it... I'm cooking");
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
        StartWalking();
    }
}
