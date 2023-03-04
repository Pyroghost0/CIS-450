/* Anna Breuker
 * Loved
 * Project 1
 * Loved recation to a flower
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loved : GraveReaction
{
    private float newRate = -.5f;
    public override float UpdateRemembrance()
    {
        return newRate;
    }
}
