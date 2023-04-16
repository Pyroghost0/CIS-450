/* Caleb Kahn
 * PlayerDecorator
 * Project 5
 * Abstract decorator class for all upgrades to the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDecorator : PlayerUpgrade
{
    public PlayerUpgrade nextAbility;
    public override abstract void UseAbillity();
}
