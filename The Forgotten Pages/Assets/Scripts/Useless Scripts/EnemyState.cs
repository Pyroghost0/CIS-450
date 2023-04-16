/* Caleb Kahn
 * EnemyState
 * Project 5
 * State that decides the behaivior of the tunnel enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    public abstract void FoundPlayer();
    public abstract void StartWalking();
}
