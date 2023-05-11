using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpscareCollider : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerMovement>().jumpscareImage.gameObject.activeSelf)
            {
                Destroy(enemy.gameObject);
            }
            else
            {
                //other.GetComponent<PlayerMovement>().StartJumpscare(enemy.jumpscareSprite);
                //enemy.StopAllCoroutines();
                //Destroy(enemy.gameObject);
                if (enemy.enemyType == EnemyType.ShyMonster || enemy.enemyType == EnemyType.TunnelMonster)
                {
                    enemy.DestroyEnemy();
                }
                else
                {
                    enemy.GetComponent<Animator>().SetFloat("Activated", 1);
                    other.GetComponent<PlayerMovement>().inJumpscare = true;
                    transform.localRotation = Quaternion.Euler(-90f, Mathf.Atan2(other.transform.position.x - transform.position.x, other.transform.position.z - transform.position.z) * 57.2958f, 0f);
                }
            }
        }
    }
}
