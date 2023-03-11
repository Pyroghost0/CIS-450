/* Anna Breuker
 * Neutral
 * Project 1
 * Neutral reaction to a flower
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : GraveReaction
{
    private float newRate = -.15f;
    public override float UpdateRemembrance()
    {
        return newRate;
    }
}
