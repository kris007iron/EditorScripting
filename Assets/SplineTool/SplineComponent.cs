using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineComponent : MonoBehaviour, ISpline
{
    //We need to implement basic interface function but without making any action
    public Vector3 GetNonUniformPoint(float t)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetPoint(float t)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetLeft(float t)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetRight(float t)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetUp(float t)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetDown(float t)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetForward(float t)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetBackward(float t)
    {
        throw new System.NotImplementedException();
    }

    public float GetLenght(float stepSize)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetControlPoint(int index)
    {
        throw new System.NotImplementedException();
    }

    public void SetControlPoint(int index, Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void InsertControlPoint(int index, Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveControlPoint(int index)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetDistance(float dsitance)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 FindClosest(Vector3 worldPoint)
    {
        throw new System.NotImplementedException();
    }

    public int ControlPointCount => throw new System.NotImplementedException();

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
}
