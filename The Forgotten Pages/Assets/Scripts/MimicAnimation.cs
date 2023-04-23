using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicAnimation : MonoBehaviour
{
    private float floatCycleTime = 1.2f;
    private float floatCycleDistence = .5f;
    private float curentYpos = -1f;
    private float timer = 0f;

    private Transform player;
    public Enemy enemy;

    private void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

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
        transform.localRotation = Quaternion.Euler(-90f, Mathf.Atan2(player.position.x - transform.position.x, player.position.z - transform.position.z) * 57.2958f, 0f);
    }
}
