/* Caleb Kahn
 * TunnelMonster
 * Project 5
 * Enemy that goes through paths and can see the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TunnelMonster : Enemy
{
    public NavMeshAgent navMeshAgent;
    public List<Transform> tunnelGraph;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    //private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Transform player;
    public bool atDespawn = false;
    public bool seesPlayer = false;
    public Transform head;

    public EnemyState currentState;
    public EnemyState walkingState;
    public EnemyState seePlayerState;

    // Start is called before the first frame update
    void Start()
    {
        //enemyType = EnemyType.TunnelMonster;
        List<List<Transform>> tunnels = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().tunnelGraph;
        for (int i = 0; i < tunnels.Count; i++)
        {
            if (Vector3.Distance(tunnels[i][0].position, transform.position) < 2.1f)
            {
                tunnelGraph = tunnels[i];
                break;
            }
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        minDespawnTime = 0f;
        StartCoroutine(EnemyBehaivior());
    }

    public void Walk()
    {
        currentState = walkingState;
        currentState.StartWalking();
    }

    public void Sighted()
    {
        currentState = seePlayerState;
        currentState.FoundPlayer();
    }

    protected override IEnumerator EnemyActionBehaivior()
    {
        walkingState = gameObject.AddComponent<WalkingAroundState>();
        seePlayerState = gameObject.AddComponent<SeePlayerState>();
        //Animation stuff;
        yield return new WaitForSeconds(2f);
        StartCoroutine(LookAround());
        Walk();
        /*Transform currentPosition = tunnelGraph[1];
        while (true)
        {
            //Walking in path
            while (!seesPlayer)
            {
                navMeshAgent.destination = currentPosition.position;
                //Debug.Log(navMeshAgent.destination);
                yield return new WaitUntil(() => seesPlayer || Vector3.Distance(currentPosition.position, transform.position) <= 3f);
                if (!seesPlayer && currentPosition.GetComponent<Position>().otherPositions.Count == 0)
                {
                    atDespawn = true;
                    yield break;
                }
                currentPosition = currentPosition.GetComponent<Position>().otherPositions[0];
            }
            //Debug.Log("Found Player");
            while (seesPlayer)
            {
                while (seesPlayer)
                {
                    Vector3 destination = player.transform.position;
                    navMeshAgent.destination = destination;
                    if (seesPlayer)
                    {
                        seesPlayer = false;
                        yield return new WaitForFixedUpdate();
                    }
                    else
                    {
                        yield return new WaitUntil(() => seesPlayer || Vector3.Distance(destination, transform.position) <= 1f);
                    }
                }
                for (int i = 0; i < 50; i++)
                {
                    if (seesPlayer)
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

            //Sets desitination to closest position relative to final position
            currentPosition = tunnelGraph[0];
            Vector3 dist = transform.position - currentPosition.position;
            float eval = (Mathf.Abs(dist.x) + Mathf.Abs(dist.z));
            for (int i = 1; i < tunnelGraph.Count; i++)
            {
                dist = transform.position - tunnelGraph[i].position;
                float newEval = (Mathf.Abs(dist.x) + Mathf.Abs(dist.z)) * (i+1f);
                if (eval <= newEval)
                {
                    currentPosition = tunnelGraph[i];
                    eval = newEval;
                }
            }
        }*/
    }

    IEnumerator LookAround()
    {
        float timer = Random.Range(0f, Mathf.PI*2f);
        while (!atDespawn)
        {
            yield return new WaitUntil(() => !frozen);
            if (seesPlayer)
            {
                Vector3 direction = player.position - transform.position;
                direction.y = 0;
                direction = direction.normalized;
                head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                yield return new WaitForFixedUpdate();
                if (!seesPlayer)
                {
                    timer = Random.Range(0f, Mathf.PI * 2f);
                    //timer = Mathf.Asin(head.rotation.);
                }
            }
            else
            {
                //Debug.Log(new Vector3(-Mathf.Abs(Mathf.Sin(timer)), 0f, Mathf.Cos(timer)));
                head.localRotation = Quaternion.Slerp(head.localRotation, Quaternion.LookRotation(new Vector3(Mathf.Sin(timer) * Mathf.Abs(Mathf.Sin(timer)), 0f, Mathf.Abs(Mathf.Cos(timer)) +.2f)), Time.deltaTime * 1.5f);
                timer += Time.deltaTime / 2f;
                yield return new WaitForFixedUpdate(); 
            }
        }
    }

    protected override IEnumerator DespawnBehaivior()
    {
        //Debug.Log("Despawning");
        yield return new WaitUntil(() => atDespawn);
        //Animation
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
