/* Caleb Kahn
 * Mimic
 * Project 5
 * Enemy that acts as an object, attacks when player gets close
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mimic : Enemy
{
    public NavMeshAgent navMeshAgent;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    private Transform player;
    public SkinnedMeshRenderer bodyMesh;
    private bool foundPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //enemyType = EnemyType.Mimic;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        minDespawnTime = 40f;
        StartCoroutine(EnemyBehaivior());
    }
    protected override IEnumerator EnemyActionBehaivior()
    {
        yield return new WaitForEndOfFrame();
        /*while (!foundPlayer)
        {
            yield return new WaitForFixedUpdate();
            if (Vector3.Distance(transform.position, player.position) < 8f)
            {
                foundPlayer = true;
            }
        }
        while (true)
        {
            navMeshAgent.SetDestination(player.position);
            yield return new WaitForFixedUpdate();
        }*/
    }

    protected override IEnumerator DespawnBehaivior()
    {
        while (!foundPlayer)
        {
            if (!camera.IsObjectVisible(bodyMesh))
            {
                //Debug.Log("Despawning");
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void DestroyEnemy()
    {
        player.GetComponent<PlayerMovement>().StartJumpscare(jumpscareSprite);
        Destroy(gameObject);
    }
}
