using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hated : GraveReaction
{
    public float newRate = 1f;
    public override float UpdateRemembrance()
    {
        return newRate;
    }
}
