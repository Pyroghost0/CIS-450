/* Caleb Kahn
 * DespawnableItem
 * Project 1
 * Item that despawns
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnableItem : Item
{
    public float despawnTime = 15f;
    public float despawnFadeTime = 2f;

    protected override void UpdateFunction()
    {
        timer += Time.deltaTime;
        float distence = floatCycleDistence * -Mathf.Sin(timer / floatCycleTime * Mathf.PI);
        transform.position += new Vector3(0f, curentYpos - distence, 0f);
        curentYpos = distence;
        if (timer > despawnTime - despawnFadeTime)
        {
            if (timer > despawnTime)
            {
                Destroy(gameObject);
            }
            else
            {
                sprite.color = new Color(1f, 1f, 1f, (despawnTime - timer) / despawnFadeTime);
            }
        }
    }
}
