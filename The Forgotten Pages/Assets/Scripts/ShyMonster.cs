using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShyMonster : Enemy
{
    public NavMeshAgent navMeshAgent;
    private List<Transform> navGraph;
    private Camera camera;
    private Transform player;
    public bool wasFound = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyType = EnemyType.ShyMonster;
        navGraph = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().navGraph;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().behindPosition;
        minDespawnTime = 0f;
        StartCoroutine(EnemyBehaivior());
    }

    protected override IEnumerator EnemyActionBehaivior()
    {
        while (!(wasFound || Vector3.Distance(player.parent.position, transform.position) < 5f))
        {
            //Debug.Log(Vector3.Distance(player.parent.position, transform.position));
            navMeshAgent.SetDestination(player.position);
            /*if (camera.IsObjectVisible(GetComponent<MeshRenderer>()))
            {
                Debug.Log("Stopping");
            }*/
            navMeshAgent.isStopped = camera.IsObjectVisible(GetComponent<MeshRenderer>());
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log("Next phase");
        navMeshAgent.isStopped = false;
        player = player.parent;
        if (Vector3.Distance(player.position, transform.position) < 5f)
        {
            while (!wasFound)
            {
                navMeshAgent.SetDestination(player.position);
                yield return new WaitForFixedUpdate();
            }
        }
        Vector3 prev = Vector3.zero;
        while (true)
        {
            //Sets initial desitination to farthest position without going near player
            Transform farthest = navGraph[0];
            //float playerDist = (player.position - farthest.position).magnitude;
            //float shyDist = (transform.position - farthest.position).magnitude;
            //float eval = (shyDist - playerDist) * shyDist;
            Vector3 playerDist = player.position - farthest.position;
            Vector3 shyDist = transform.position - farthest.position;
            float eval = ((Mathf.Abs(playerDist.x) - Mathf.Abs(shyDist.x)) * Mathf.Abs(shyDist.x)) + ((Mathf.Abs(playerDist.z) - Mathf.Abs(shyDist.z)) * Mathf.Abs(Mathf.Abs(shyDist.z)));
            if (farthest.position == prev)
            {
                eval = -99999f;
            }
            //Debug.Log("Player: " + playerDist + ",\tShy: " + shyDist + ",\tEval: " + eval + "\n" + farthest.position);
            for (int i = 1; i < navGraph.Count; i++)
            {
                //playerDist = (player.position - navGraph[i].position).magnitude;
                //shyDist = (transform.position - navGraph[i].position).magnitude;
                //float newEval = (shyDist - playerDist) * shyDist;
                playerDist = player.position - navGraph[i].position;
                shyDist = transform.position - navGraph[i].position;
                float newEval = ((Mathf.Abs(playerDist.x) - Mathf.Abs(shyDist.x)) * Mathf.Abs(shyDist.x)) + ((Mathf.Abs(playerDist.z) - Mathf.Abs(shyDist.z)) * Mathf.Abs(Mathf.Abs(shyDist.z)));
                if (navGraph[i].position == prev)
                {
                    newEval = -99999f;
                }
                //Debug.Log("Player: " + playerDist + ",\tShy: " + shyDist + ",\tEval: " + newEval + "\n" + navGraph[i].position);
                //if ((player.position - farthest.position).magnitude < (player.position - navGraph[i].position).magnitude)
                if (newEval > eval)
                {
                    farthest = navGraph[i];
                    eval = newEval;
                }
            }
            navMeshAgent.destination = farthest.position;
            prev = farthest.position;
            //Walking in path
            while (Vector3.Distance(farthest.position, transform.position) > 3f)
            {
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator DespawnBehaivior()
    {
        //Debug.Log("Despawning");
        yield return new WaitUntil(() => wasFound);
        float timer = 0f;
        while (timer < 15f)//Waits 15 seconds (and player isn't looking at it) or until hidden from player
        {
            RaycastHit[] rayHits = Physics.RaycastAll(transform.position, player.position - transform.position);
            //Debug.DrawRay(transform.position, player.position - transform.position);
            bool wall = false;
            for (int i = 0; i < rayHits.Length; i++)
            {
                if (rayHits[i].collider != null)
                {
                    //Debug.Log(rayHits[i].collider.name);
                    if (rayHits[i].collider.CompareTag("Player"))
                    {
                        break;
                    }
                    else
                    {
                        wall = true;
                    }
                }
            }
            if (wall && !camera.IsObjectVisible(GetComponent<MeshRenderer>()))
            {
                break;
            }
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        yield return new WaitUntil(() => !camera.IsObjectVisible(GetComponent<MeshRenderer>()));
        //Debug.Log("Out of view");
        Destroy(gameObject);
    }
}
