/* Caleb Kahn
 * ItemFactory
 * Project 1
 * Creates Item Objects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public void SpawnItem(ItemType itemType, Vector3 spawnPosition, bool despawnable, float despawnTime, ItemCreator itemCreator)
    {
        if (despawnable)
        {
            DespawnableItem item = Instantiate(itemCreator.despawnableItemPrefab, spawnPosition, itemCreator.transform.rotation).GetComponent<DespawnableItem>();
            item.itemType = itemType;
            item.sprite.sprite = itemCreator.itemSprites[(int)itemType];
            item.despawnTime = despawnTime;
        }
        else
        {
            PermanentItem item = Instantiate(itemCreator.despawnableItemPrefab, spawnPosition, itemCreator.transform.rotation).GetComponent<PermanentItem>();
            item.itemType = itemType;
            item.sprite.sprite = itemCreator.itemSprites[(int)itemType];
        }
    }
}
