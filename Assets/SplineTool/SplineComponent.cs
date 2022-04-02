using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineComponent : MonoBehaviour, ISpline
{
    #region lazyIndexing

    /// <summary>
    /// Index is used to provide uniform point searching
    /// </summary>
    private SplineIndex uniformIndex;
    SplineIndex Index
    {
        get
        {
            if (uniformIndex == null) uniformIndex = new SplineIndex(this);
            return uniformIndex;
        }
    }

    public void ResetIndex()
    {
        uniformIndex = null;
        length = null;
    }

    #endregion
    //Some variables to store the data
    public bool closed = false;
    public List<Vector3> points = new List<Vector3>();
    // Question mark checks if variable is not null cuz float is nullable
    public float? length;

    #region InterfaceImplement

    //We need to implement basic interface function but without making any action
    public Vector3 GetNonUniformPoint(float t)
    {
        //Based on amount of points function will do different things 
        switch (points.Count)
        {
            case 0:
                return Vector3.zero;
            case 1:
                return transform.TransformPoint(points[0]);
            case 2:
                return transform.TransformPoint(Vector3.Lerp(points[0], points[1], t));//Lerp is used for linear interpolation
            case 3:
                return transform.TransformPoint(points[1]);
            default:
                return Hermite(t);
        }
    }

    public Vector3 GetPoint(float t) => Index.GetPoint(t);

    public Vector3 GetLeft(float t) => -GetRight(t);

    public Vector3 GetRight(float t)
    {
        var A = GetPoint(t - 0.001f);
        var B = GetPoint(t + 0.001f);
        var delta = (B - A);
        return new Vector3(-delta.z, 0, delta.x).normalized;
    }

    public Vector3 GetUp(float t)
    {
        var A = GetPoint(t - 0.001f);
        var B = GetPoint(t + 0.001f);
        var delta = (B - A).normalized;
        return Vector3.Cross(delta, GetRight(t));
    }

    public Vector3 GetDown(float t) => -GetUp(t);

    public Vector3 GetForward(float t)
    {
        var A = GetPoint(t - 0.001f);
        var B = GetPoint(t + 0.001f);
        return (B - A).normalized;
    }

    public Vector3 GetBackward(float t) => -GetForward(t);

    public float GetLength
        (float step = 0.001f)
    {
        var D = 0f;
        var A = GetNonUniformPoint(0);
        for (var t = 0f; t < 1f; t += step)
        {
            var B = GetNonUniformPoint(t);
            var delta = (B - A);
            D += delta.magnitude;
            A = B;
        }

        return D;
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    public void SetControlPoint(int index, Vector3 position)
    {
        ResetIndex();
        points[index] = position;
    }

    public void InsertControlPoint(int index, Vector3 position)
    {
        ResetIndex();
        if (index >= points.Count)
            points.Add(position);
        else
            points.Insert(index, position);
    }

    public void RemoveControlPoint(int index)
    {
        ResetIndex();
        points.RemoveAt(index);
    }

    public Vector3 GetDistance(float distance)
    {
        if (length == null) length = GetLength();
        return uniformIndex.GetPoint(distance / length.Value);
    }

    //Return the approximate closest postion on the spline to a world porint, onl numerical soultion due to the nature of splines. We divide spline into 1024 points and we choose th closest by using sqrMagnitude
    public Vector3 FindClosest(Vector3 worldPoint)
    {
        var smallestDelta = float.MaxValue;
        var step = 1f / 1024;
        var closestPoint = Vector3.zero;
        for (int i = 0; i <= 1024; i++)
        {
            var p = GetPoint(i * step);
            var delta = (worldPoint - p).sqrMagnitude;
            if(delta < smallestDelta)
            {
                closestPoint = p;
                smallestDelta = delta;
            }
        }
        return closestPoint;
    }


    #endregion

    //Shorter version of simple return method
    public int ControlPointCount => points.Count;

    //We use interpolation to count estimated position of line based on actual data of points
    internal static Vector3 Interpolate(Vector3 a, Vector3 b, Vector3 c,Vector3 d, float u)
    {
        return(0.5f *
               (
                   (-a + 3f * b - 3f * c + d) *
                   (u * u * u) +
                   (2f * a - 5f * b + 4f * c - d) *
                   (u * u) +
                   (-a + c) *
                   u + 2f * b
               ));
    }

    //Return interpolated world position of control points
    private Vector3 Hermite(float t)
    {
        var count = points.Count - (closed ? 0 : 3);
        var i = Mathf.Min(Mathf.FloorToInt(t * (float)count), count - 1);
        var u = t * (float)count - (float)i;
        var a = GetPointByIndex(i);
        var b = GetPointByIndex(i + 1);
        var c = GetPointByIndex(i + 2);
        var d = GetPointByIndex(i + 3);
        return transform.TransformPoint((Interpolate(a, b, c, d, u)));
    }
    Vector3 GetPointByIndex(int i)
    {
        if (i < 0) i += points.Count;
        return points[i % points.Count];
    }
}
