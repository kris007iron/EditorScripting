using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public Rigidbody projectile;
    public Vector3 offset = Vector3.forward;
    //By range we can add slider beneath velocity variable in inspector, values from 0 to 100
    [Range(0, 100)]public float velocity = 10;

    //As long as method do not have arguments we can simply call it from context menu
    [ContextMenu("Fire")]
    public void Fire()
    {
        var body = Instantiate(
            projectile,
            transform.TransformPoint(offset),
            transform.rotation);
        body.velocity = Vector3.forward * velocity;
    }
}
