using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineIndex : MonoBehaviour
{
    /// <summary>
    /// We cant store distance from different points along the spline so we create an index of discrete uniform positions along the whole spline
    /// Class provides the uniform positions assumed by the interface
    /// </summary>
    public Vector3[] linearPoints;
    private SplineComponent spline;

    public int ControlPointCount => spline.ControlPointCount;

    public SplineIndex(SplineComponent spline)
    {
        this.spline = spline;
        ReIndex();
    }

    public void ReIndex()
    {
        var searchStepSize = 0.00001f;
        var lentgth = spline.GetLenght(searchStepSize);
        var indexSize = Mathf.FloorToInt(lentgth * 2);
        var _linearPoints = new List<Vector3>(indexSize);
        var t = 0f;


        var linearDistanceStep = lentgth / 1024;
        var linearDistanceStep2 = Math.Pow(linearDistanceStep, 2);
        var start = spline.GetNonUniformPoint(0);
        _linearPoints.Add(start);
        while (t <= 1f)
        {
            var current = spline.GetNonUniformPoint(t);
            while ((current - start).sqrMagnitude <= linearDistanceStep2)
            {
                t += searchStepSize;
                current = spline.GetNonUniformPoint(t);
            }

            start = current;
            _linearPoints.Add(current);
        }

        linearPoints = _linearPoints.ToArray();
    }

    public Vector3 GetPoint(float t)
    {
        var section = linearPoints.Length - (spline.closed ? 0 : 3);
        var i = Mathf.Min(Mathf.FloorToInt(t * (float) section), section - 1);
        var count = linearPoints.Length;
        if (i < 0) i += count;
        var u = t * (float) section - (float) i;
        var a = linearPoints[(i + 0) % count];
        var b = linearPoints[(i + 1) % count];
        var c = linearPoints[(i + 2) % count];
        var d = linearPoints[(i + 3) % count];
        return SplineComponent.Interpolate(a, b, c, d, u);

    }
}
