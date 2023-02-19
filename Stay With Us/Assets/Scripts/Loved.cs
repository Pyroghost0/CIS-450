using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loved : GraveReaction
{
    public float pointsWorth = 20;
    public override float UpdateRemembrance()
    {
        return pointsWorth;
    }
}
