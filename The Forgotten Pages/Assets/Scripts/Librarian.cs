/* Caleb Kahn
 * Librarian
 * Project 5
 * Enemy that can hear sounds
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Librarian : Enemy
{
    public NavMeshAgent navMeshAgent;
    private Transform previousPosition;
    private Position currentPosition;
    private List<Transform> navGraph;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    private PlayerMovement player;
    private bool foundPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //enemyType = EnemyType.Librarian;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LibrarianInArea();
        navGraph = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().navGraph;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        minDespawnTime = 20f;
        StartCoroutine(EnemyBehaivior());
    }

    protected override IEnumerator EnemyActionBehaivior()
    {
        StartCoroutine(DetectSound());

        while (true)
        {
            //Sets initial desitination to closest position
            currentPosition = navGraph[0].GetComponent<Position>();
            for (int i = 1; i < navGraph.Count; i++)
            {
                if ((transform.position - currentPosition.transform.position).magnitude > (transform.position - navGraph[i].position).magnitude)
                {
                    currentPosition = navGraph[i].GetComponent<Position>();
                }
            }
            previousPosition = null;

            //Walking in path
            while (!foundPlayer)
            {
                //Debug.Log(navMeshAgent.destination);
                navMeshAgent.destination = currentPosition.transform.position;
                yield return new WaitUntil(() => foundPlayer || Vector3.Distance(currentPosition.transform.position, transform.position) <= 3f);
                Transform position = currentPosition.transform;
                //Picks a random next position (if it contains the previous position, it'll pick a different one
                currentPosition = currentPosition.otherPositions.Count != 1 && currentPosition.otherPositions.Contains(previousPosition) ? currentPosition.otherPositions[(currentPosition.otherPositions.FindIndex(x => x.Equals(previousPosition)) + Random.Range(1, currentPosition.otherPositions.Count)) % currentPosition.otherPositions.Count].GetComponent<Position>() : currentPosition.otherPositions[Random.Range(0, currentPosition.otherPositions.Count)].GetComponent<Position>();
                previousPosition = position;
            }
            //Debug.Log("Found Player");
            while (foundPlayer)
            {
                while (foundPlayer)
                {
                    Vector3 destination = player.transform.position;
                    navMeshAgent.destination = destination;
                    if (foundPlayer)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    else
                    {
                        yield return new WaitUntil(() => foundPlayer || Vector3.Distance(destination, transform.position) <= 1f);
                    }
                }
                for (int i = 0; i < 50; i++)
                {
                    if (foundPlayer)
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
        }
    }

    IEnumerator DetectSound()
    {
        while (true)
        {
            RaycastHit[] rayHits = Physics.RaycastAll(transform.position, player.transform.position - transform.position, player.soundRadius);
            //Debug.DrawRay(transform.position, player.transform.position - transform.position);
            int walls = 0;
            bool found = false;
            for (int i = 0; i < rayHits.Length; i++)
            {
                if (rayHits[i].distance > player.soundRadius / walls)
                {
                    break;
                }
                if (rayHits[i].collider != null)
                {
                    //Debug.Log(rayHits[i].collider.name);
                    if (rayHits[i].collider.CompareTag("Player"))
                    {
                        found = true;
                        break;
                    }
                    else
                    {
                        walls++;
                    }
                }
            }
            foundPlayer = found;
            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator DespawnBehaivior()
    {
        //Debug.Log("Despawning");
        yield return new WaitForFixedUpdate();
        //Renderer works with a spriteRender or any type of renderer, but it doesn't work well, and the camera one is munually put in with the CameraSite class which is an added extention to the camera class
        //Debug.Log("Mesh: " + camera.IsObjectVisible(GetComponent<MeshRenderer>()));
        //Debug.Log("Renderer" + GetComponent<MeshRenderer>().isVisible);
        yield return new WaitUntil(() => !camera.IsObjectVisible(GetComponent<MeshRenderer>()));
        //Debug.Log("Out of view");
        Destroy(gameObject);
    }
}
