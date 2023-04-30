/* Caleb Kahn
 * Enemy
 * Project 5
 * Abstract class for all types of enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public float sanityDamage = .25f;
    public bool frozen = false;
    public GameObject jumpscareCollider;
    //private float frozenSpeed;

    public Sprite jumpscareSprite;

    protected IEnumerator EnemyBehaivior() {
        StartCoroutine(EnemyActionBehaivior());
        yield return new WaitForSeconds(minDespawnTime);
        StartCoroutine(DespawnBehaivior());
    }

    public virtual void Freeze()
    {
        frozen = true;
        GetComponent<NavMeshAgent>().isStopped = true;
        jumpscareCollider.SetActive(false);
        //frozenSpeed = GetComponent<NavMeshAgent>().speed;
    }

    public virtual void Unfreeze()
    {
        frozen = false;
        GetComponent<NavMeshAgent>().isStopped = false;
        jumpscareCollider.SetActive(true);
    }

    public void DestroyEnemy()
    {
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.RemoveSanity(sanityDamage);
        player.StartJumpscare(jumpscareSprite);
        player.inJumpscare = false;
        Destroy(gameObject);
    }

    protected abstract IEnumerator EnemyActionBehaivior();
    protected abstract IEnumerator DespawnBehaivior();
}
