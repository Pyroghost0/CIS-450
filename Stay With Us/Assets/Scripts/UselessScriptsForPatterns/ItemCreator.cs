/* Caleb Kahn
 * ItemCreator
 * Project 1
 * Creates Item Objects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public GameObject despawnableItemPrefab;
    public GameObject permanentItemPrefab;
    public Sprite[] itemSprites;

    public void SpawnItem(ItemType itemType, Vector3 spawnPosition, bool despawnable, float despawnTime=30f)
    {
        gameObject.GetComponent<ItemFactory>().SpawnItem(itemType, spawnPosition, despawnable, despawnTime, this);
    }
}
