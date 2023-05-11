using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollAnimation : MonoBehaviour
{
    private float floatCycleTime = 1.2f;
    private float floatCycleDistence = .5f;
    private float curentYpos = -1f;
    private float timer = 0f;

    private Transform player;
    public RagdollMonster enemy;
    public Rigidbody mainRB;

    private float previousYPos;
    Quaternion rootRotation;

    public bool seen = false;

    private void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!enemy.frozen)
        {
            timer += Time.deltaTime;
            /*if (timer > floatCycleTime * Mathf.PI * 2f)
            {
                timer -= floatCycleTime * Mathf.PI * 2f;
            }*/
            if ((player.position - mainRB.transform.position).magnitude < 10)
            {
                seen = true;
                float distence = floatCycleDistence * 2 * -Mathf.Sin(timer / floatCycleTime * enemy.stareAmount * Mathf.PI);
                transform.rotation = Quaternion.identity;
                mainRB.transform.rotation = rootRotation;
                mainRB.isKinematic = true;
                transform.position = new Vector3(transform.position.x, previousYPos + 2.5f + curentYpos - distence, transform.position.z + curentYpos - distence);
                curentYpos = distence;
                mainRB.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(player.position.x - mainRB.transform.position.x, player.position.z - mainRB.transform.position.z) * 57.2958f, 0f);
            }
            else if ((player.position - mainRB.transform.position).magnitude > 15f || !enemy.camera.IsObjectVisible(enemy.bodyMesh))
            {
                seen = false;
                float distence = floatCycleDistence * -Mathf.Sin(timer / floatCycleTime * Mathf.PI);
                mainRB.isKinematic = false;
                transform.position += new Vector3(0f, curentYpos - distence, 0f);
                curentYpos = distence;
                previousYPos = transform.position.y;

            }
        }
    }
}
