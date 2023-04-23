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
                other.GetComponent<PlayerMovement>().RemoveSanity(enemy.sanityDamage);
                //other.GetComponent<PlayerMovement>().StartJumpscare(enemy.jumpscareSprite);
                //enemy.StopAllCoroutines();
                enemy.GetComponent<Animator>().SetFloat("Activated", 1);
                //Destroy(enemy.gameObject);
            }
        }
    }
}
