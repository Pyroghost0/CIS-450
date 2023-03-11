/* Anna Breuker
 * Hated
 * Project 1
 * Hated reaction to flower
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hated : GraveReaction
{
    public float newRate = .75f;
    public override float UpdateRemembrance()
    {
        return newRate;
    }
}
