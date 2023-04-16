/* Caleb Kahn
 * RagdollMonster
 * Project 5
 * Enemy that when looked at, becomes more aggresive *Can't move*
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollMonster : Enemy
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    private Transform player;
    public float stareAmount = 0f;
    public GameObject body;
    public MeshRenderer bodyMesh;
    public AudioSource scream;

    // Start is called before the first frame update
    void Start()
    {
        //enemyType = EnemyType.TunnelMonster;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        minDespawnTime = scream.clip.length * 2f;
        StartCoroutine(EnemyBehaivior());
    }

    protected override IEnumerator EnemyActionBehaivior()
    {
        while (true)
        {
            //Debug.Log("Camera: " + camera.IsObjectVisible(bodyMesh));
            //Debug.Log("Mesh: " + bodyMesh.isVisible);
            if (camera.IsObjectVisible(bodyMesh))
            {
                //Debug.Log(1);
                RaycastHit rayHit;
                //Debug.DrawRay(transform.position, (player.position - transform.position) * 100f, Color.red);
                if (Physics.Raycast(transform.position, player.position - transform.position, out rayHit) && rayHit.collider != null && rayHit.collider.CompareTag("Player"))
                {
                    //Debug.Log("Seen");
                    stareAmount += Time.deltaTime / 6f;
                    if (stareAmount >= 1f)
                    {
                        Debug.Log("Attack");
                    }
                    else
                    {
                        scream.volume = stareAmount;
                    }
                }
                /*else
                {
                    Debug.Log("Collider: " + (rayHit.collider != null ? rayHit.collider.name : "Fail"));
                }*/
            }
            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator DespawnBehaivior()
    {
        while (true)
        {
            if (!camera.IsObjectVisible(bodyMesh))
            {
                //Debug.Log("Despawning");
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(scream.clip.length);
        }
    }
}