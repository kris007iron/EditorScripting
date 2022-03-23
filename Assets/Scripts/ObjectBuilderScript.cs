using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilderScript : MonoBehaviour
{
    public GameObject obj;
    public Vector3 spawnPoint;
    //Simple example class which can spawn given gameObject at given position
    public void BuildObject()
    {
        Instantiate(obj, spawnPoint, Quaternion.identity);
    }
}
