using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A interface for general spline data.
/// NB: - All Vector3 arguments and Vector3 return values are in world space.
///     - All t arguments specify a uniform position along the spline, apart from the GetNonUniformPoint method.
/// </summary>
public interface ISpline
{
    Vector3 GetNonUniformPoint(float t); 
    Vector3 GetPoint(float t);


    Vector3 GetLeft(float t);
    Vector3 GetRight(float t);
    Vector3 GetUp(float t);
    Vector3 GetDown(float t);
    Vector3 GetForward(float t);
    Vector3 GetBackward(float t);

    float GetLenght(float stepSize);


    Vector3 GetControlPoint(int index);
    void SetControlPoint(int index, Vector3 position);
    void InsertControlPoint(int index, Vector3 position);
    void RemoveControlPoint(int index);

    Vector3 GetDistance(float dsitance);
    Vector3 FindClosest(Vector3 worldPoint);
    int ControlPointCount { get; }
}
