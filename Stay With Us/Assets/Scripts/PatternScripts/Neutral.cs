using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : GraveReaction
{
    private float newRate = 0f;
    public override float UpdateRemembrance()
    {
        return newRate;
    }
}
