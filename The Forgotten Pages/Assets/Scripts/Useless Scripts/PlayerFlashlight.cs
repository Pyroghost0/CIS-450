/* Caleb Kahn
 * PlayerFlashlight
 * Project 5
 * The flashlight ability for the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : PlayerDecorator
{
    public Light light;

    public PlayerFlashlight(PlayerUpgrade playerUpgrade, Light flashlight)
    {
        light = flashlight;
        nextAbility = playerUpgrade;
    }

    public override void UseAbillity()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            light.enabled = !light.enabled;
        }
        nextAbility.UseAbillity();
    }
}
