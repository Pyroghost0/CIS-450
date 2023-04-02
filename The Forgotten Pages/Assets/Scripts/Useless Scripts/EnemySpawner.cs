/* Caleb Kahn
 * EnemySpawner
 * Project 5
 * Spawns whichever enemy specified
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;

    public void SpawnEnemy(Vector3 position, EnemyType enemyType)
    {
        Instantiate(enemies[(int)enemyType], position, transform.rotation);
    }
}
