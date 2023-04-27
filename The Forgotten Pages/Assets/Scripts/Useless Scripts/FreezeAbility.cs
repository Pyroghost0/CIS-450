/* Caleb Kahn
 * FreezeAbility
 * Project 5
 * The flashlight ability for the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlreezeAbility : PlayerDecorator
{
    PlayerMovement playerMovement;

    public FlreezeAbility(PlayerUpgrade playerUpgrade, PlayerMovement pm)
    {
        playerMovement = pm;
        nextAbility = playerUpgrade;
    }

    public override void UseAbillity()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerMovement.freezeUsable && !GameController.instance.isInMemory)
        {
            playerMovement.Freeze();
        }
        nextAbility.UseAbillity();
    }
}
