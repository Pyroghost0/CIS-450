using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : GraveReaction
{
    public float pointsWorth = 10;
    public override float UpdateRemembrance()
    {
        return pointsWorth;
    }
}
