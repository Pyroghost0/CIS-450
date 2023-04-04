/* Caleb Kahn
 * Enemy
 * Project 5
 * Abstract class for all types of enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Librarian = 0,//Detects sound, Doesn't know the players location unless they run
    ShyMonster = 1,//Knows player location, walks behind player, easily to defeat if you look around constantly since you only need to look at it once
    TunnelMonster = 2,//Most like a normal enemy, doesn't know player location, can spawn in front of the player *in front of tunnels*, it detects the player by seeing them, walks to an from tunnel locations
    RagdollMonster = 3,//Doesn't move, but stares at the player, mostly for atmosphere
    Mimic = 4//The mimic lures the player in by looking like a memory or common object with a detectable differance, only will attack if the player gets close
}

public abstract class Enemy : MonoBehaviour
{
    protected float minDespawnTime = 20f;
    public EnemyType enemyType;


    protected IEnumerator EnemyBehaivior() {
        StartCoroutine(EnemyActionBehaivior());
        yield return new WaitForSeconds(minDespawnTime);
        StartCoroutine(DespawnBehaivior());
    }

    protected abstract IEnumerator EnemyActionBehaivior();
    protected abstract IEnumerator DespawnBehaivior();
}
