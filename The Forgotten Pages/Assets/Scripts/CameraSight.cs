/* Caleb Kahn
 * CameraSight
 * Project 5
 * Extended class for camera
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraSight
{
    //Note: Extension methods can add to an existing class, but they cannot override existing methods
    //Note: Functions must be static, Functions must be declared inside a non-generic, non-nested, static class, and Functions include a special syntax for their first argument, which is a reference to the calling object
    public static bool IsObjectVisible(this UnityEngine.Camera @this, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
    }
}