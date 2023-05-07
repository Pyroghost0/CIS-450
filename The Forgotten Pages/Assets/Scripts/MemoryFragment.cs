/* Caleb Kahn, Anna Breuker, Cooper Denault
 * MemoryFragment
 * Project 5
 * The object that triggers the cutscene
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment : MonoBehaviour
{
    private float floatCycleTime = 1.2f;
    private float floatCycleDistence = .5f;
    private float curentYpos = 0f;
    private float timer = 0f;
    Transform player;

    public GameController gameController;

    public int memoryNum;

    public PlayerMovement playerMove;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameController = gameController.GetComponent<GameController>();
        playerMove = playerMove.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float distence = floatCycleDistence * -Mathf.Sin(timer / floatCycleTime * Mathf.PI);
        transform.position += new Vector3(0f, curentYpos - distence, 0f);
        curentYpos = distence;
        /*if (timer > floatCycleTime * Mathf.PI * 2f)
        {
            timer -= floatCycleTime * Mathf.PI * 2f;
        }*/
        transform.localRotation = Quaternion.Euler(-45f, Mathf.Atan2(player.position.x - transform.position.x, player.position.z - transform.position.z) * 57.2958f + 180f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            playerMove.pickupSound.Play();

            GameController.instance.SwitchGameMode(memoryNum);
            gameController.breathing.Stop();
            gameController.heartBeat.Stop();

            Destroy(gameObject);

        }
    }
}
