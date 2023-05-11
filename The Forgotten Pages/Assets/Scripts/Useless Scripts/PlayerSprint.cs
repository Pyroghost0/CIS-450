/* Caleb Kahn
 * PlayerSprint
 * Project 5
 * The sprinting ability for the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprint : PlayerDecorator
{
    public PlayerMovement playerMovement;
    public float initSpeed;

    public PlayerSprint(PlayerUpgrade playerUpgrade, PlayerMovement pm)
    {
        playerMovement = pm;
        playerMovement.playerSprint = this;
        initSpeed = pm.speed;
        nextAbility = playerUpgrade;
    }

    public override void UseAbillity()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            playerMovement.sprinting = true;
            playerMovement.speed = initSpeed * 2;
        }
        else
        {
            playerMovement.sprinting = false;
            playerMovement.speed = initSpeed;
        }
        nextAbility.UseAbillity();
    }
}
