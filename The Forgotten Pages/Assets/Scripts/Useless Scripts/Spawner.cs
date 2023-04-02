/* Caleb Kahn
 * Spawner
 * Project 5
 * Spawns whatever is specified
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    public void SpawnEnemy(Vector3 position, EnemyType enemyType)
    {
        enemySpawner.SpawnEnemy(position, enemyType);
    }
}
