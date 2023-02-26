using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : GraveReaction
{
    private float pointsWorth = 25f;
    public override float UpdateRemembrance()
    {
        return pointsWorth;
    }
}
