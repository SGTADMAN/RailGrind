using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class RailScript : MonoBehaviour
{
    public bool normalDir;
    public SplineContainer railSpline;
    public float totalSplineLength;

    private void Start()
    {
        railSpline = GetComponent<SplineContainer>();
        totalSplineLength = railSpline.CalculateLength();
    }

    /// <summary>
    /// Converts local float3 positions to Vector3 world positions.
    /// </summary>
    /// <param name="localPoint">float3 local position</param>
    /// <returns>Vector3 world position</returns>
    public Vector3 LocalToWorldConversion(float3 localPoint)
    {
        Vector3 worldPos = transform.TransformPoint(localPoint);
        return worldPos;
    }
    /// <summary>
    /// Converts Vector3 world positions to local float3 positions
    /// </summary>
    /// <param name="worldPoint">Vector3 world position</param>
    /// <returns>float3 local position</returns>
    public float3 WorldToLocalConversion(Vector3 worldPoint)
    {
        float3 localPos = transform.InverseTransformPoint(worldPoint);
        return localPos;
    }
    /// <summary>
    /// Calculates the normalised time value for the rail's spline by evlating the player's position.
    /// </summary>
    /// <param name="playerPos">Vector3 position for the Player</param>
    /// <param name="worldPosOnSpline">Vector3 position for point on the spline the player is closest to.</param>
    /// <returns>float time - Normalised time value between 0 & 1.</returns>
    public float CalculateTargetRailPoint(Vector3 playerPos, out Vector3 worldPosOnSpline)
    {
        float3 nearestPoint;
        float time;
        SplineUtility.GetNearestPoint(railSpline.Spline, WorldToLocalConversion(playerPos), out nearestPoint, out time);
        worldPosOnSpline = LocalToWorldConversion(nearestPoint);
        return time;
    }
    /// <summary>
    /// Calculates the direction the player is going on the rail based on their direction during the initial collision.
    /// </summary>
    /// <param name="railForward">The forward of the point on the spline.</param>
    /// <param name="playerForward">The player's forward.</param>
    public void CalculateDirection(float3 railForward, Vector3 playerForward)
    {
        //This calculates the severity of the angle between the player's forward and the forward of the point on the spline.
        //90 degrees is the cutoff point as it's the perpendicular to the rail. Anything more than that and the player is clearly
        //facing the other direction to the rail point.
        float angle = Vector3.Angle(railForward, playerForward.normalized);
        if (angle > 90f)
            normalDir = false;
        else
            normalDir = true;
    }
}
