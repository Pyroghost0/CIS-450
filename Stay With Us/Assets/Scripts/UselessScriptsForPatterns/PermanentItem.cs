/* Caleb Kahn
 * PermanentItem
 * Project 1
 * Item that doesn't despawn
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentItem : Item
{
    protected override void UpdateFunction()
    {
        timer += Time.deltaTime;
        float distence = floatCycleDistence * -Mathf.Sin(timer / floatCycleTime * Mathf.PI);
        transform.position += new Vector3(0f, curentYpos - distence, 0f);
        curentYpos = distence;
        if (timer > floatCycleTime * Mathf.PI * 2f)
        {
            timer -= floatCycleTime * Mathf.PI * 2f;
        }
    }
}
