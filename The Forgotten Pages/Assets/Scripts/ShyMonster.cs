/* Caleb Kahn
 * ShyMonster
 * Project 5
 * Enemy that can sneaks up on player and will leave once seen
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShyMonster : Enemy
{
    public NavMeshAgent navMeshAgent;
    private List<Transform> navGraph;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    private Transform player;
    public bool wasFound = false;
    private bool previousStopState;
    public Animator animator;
    public SkinnedMeshRenderer meshRenderer;
    public Material normalMat;
    public Material frozenMat;

    // Start is called before the first frame update
    void Start()
    {
        //enemyType = EnemyType.ShyMonster;
        animator = GetComponent<Animator>();
        navGraph = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().navGraph;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().behindPosition;
        minDespawnTime = 0f;
        StartCoroutine(EnemyBehaivior());
    }

    void Update()
    {
        if (navMeshAgent.isStopped)
        {
            meshRenderer.material = frozenMat;
            animator.speed = 0f;
        }
        else
        {
            meshRenderer.material = normalMat;
            animator.speed = 1f;
        }
    }

    protected override IEnumerator EnemyActionBehaivior()
    {
        StartCoroutine(PlayerSightBug());
        while (!(wasFound || Vector3.Distance(player.parent.position, transform.position) < 5f))
        {
            //Debug.Log(Vector3.Distance(player.parent.position, transform.position));
            navMeshAgent.SetDestination(player.position);
            /*if (camera.IsObjectVisible(meshRenderer))
            {
                Debug.Log("Stopping");
            }*/
            navMeshAgent.isStopped = camera.IsObjectVisible(meshRenderer);
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => !frozen);
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

    IEnumerator PlayerSightBug()
    {
        float inSightTime = 0f;
        while (!wasFound)
        {
            if (camera.IsObjectVisible(meshRenderer) && !frozen)
            {
                RaycastHit rayHit;
                if (Physics.Raycast(transform.position, player.position - transform.position, out rayHit) && rayHit.collider != null && rayHit.collider.CompareTag("Player"))
                {
                    inSightTime += Time.deltaTime;
                    if (inSightTime >= 2f)
                    {
                        wasFound = true;
                    }
                }
                else
                {

                    inSightTime = 0f;
                }
            }
            else
            {
                inSightTime = 0f;
            }
            //Debug.Log(1);
            //Debug.DrawRay(transform.position, (player.position - transform.position) * 100f, Color.red);
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log(2);
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
            if (wall && !camera.IsObjectVisible(meshRenderer))
            {
                break;
            }
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        yield return new WaitUntil(() => !camera.IsObjectVisible(meshRenderer));
        //Debug.Log("Out of view");
        Destroy(gameObject);
    }

    public override void Freeze()
    {
        frozen = true;
        previousStopState = navMeshAgent.isStopped;
        navMeshAgent.isStopped = true;
        jumpscareCollider.SetActive(false);
    }

    public override void Unfreeze()
    {
        frozen = false;
        navMeshAgent.isStopped = previousStopState;
        jumpscareCollider.SetActive(true);
    }
}
