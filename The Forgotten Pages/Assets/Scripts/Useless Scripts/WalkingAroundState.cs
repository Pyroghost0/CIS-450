/* Caleb Kahn
 * WalkingAroundState
 * Project 5
 * State that decides the behaivior of the tunnel enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAroundState : EnemyState
{
    public TunnelMonster tunnelMonster;

    public void Start()
    {
        tunnelMonster = gameObject.GetComponent<TunnelMonster>();
    }

    public override void StartWalking()
    {
        StartCoroutine(WalkCoroutine());
    }

    public IEnumerator WalkCoroutine()
    {
        //Transform currentPosition = tunnelMonster.tunnelGraph[1];
        //Sets desitination to closest position relative to final position
        Transform currentPosition = tunnelMonster.tunnelGraph[0];
        Vector3 dist = transform.position - currentPosition.position;
        float eval = (Mathf.Abs(dist.x) + Mathf.Abs(dist.z));
        for (int i = 1; i < tunnelMonster.tunnelGraph.Count; i++)
        {
            dist = transform.position - tunnelMonster.tunnelGraph[i].position;
            float newEval = (Mathf.Abs(dist.x) + Mathf.Abs(dist.z)) * (i + 1f);
            if (eval <= newEval)
            {
                currentPosition = tunnelMonster.tunnelGraph[i];
                eval = newEval;
            }
        }
        while (!tunnelMonster.seesPlayer)
        {
            tunnelMonster.navMeshAgent.destination = currentPosition.GetComponent<Position>().otherPositions.Count == 0 ? currentPosition.GetChild(0).position : currentPosition.position;
            //Debug.Log(navMeshAgent.destination);
            yield return new WaitUntil(() => tunnelMonster.seesPlayer || Vector3.Distance(currentPosition.position, transform.position) <= 3f);
            if (!tunnelMonster.seesPlayer && currentPosition.GetComponent<Position>().otherPositions.Count == 0)
            {
                tunnelMonster.atDespawn = true;
                yield break;
            }
            currentPosition = currentPosition.GetComponent<Position>().otherPositions[0];
        }
        FoundPlayer();
    }

    public override void FoundPlayer()
    {
        tunnelMonster.Sighted();
    }
}
