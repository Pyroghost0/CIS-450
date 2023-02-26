using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loved : GraveReaction
{
    private float pointsWorth = 50f;
    public override float UpdateRemembrance()
    {
        return pointsWorth;
    }
}
