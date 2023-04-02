/* Caleb Kahn
 * Enemy
 * Project 5
 * Abstract class for all types of enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Librarian = 0
}

public abstract class Enemy : MonoBehaviour
{
    protected float minDespawnTime = 20f;
    protected EnemyType enemyType;


    IEnumerator EnemyBehaivior() {
        StartCoroutine(EnemyActionBehaivior());
        yield return new WaitForSeconds(minDespawnTime);
        StartCoroutine(DespawnBehaivior());
    }

    protected abstract IEnumerator EnemyActionBehaivior();
    protected abstract IEnumerator DespawnBehaivior();
}
