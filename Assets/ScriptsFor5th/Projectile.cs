using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//There you by default to game obj to whom the script is attached to we add a Rigidbody component
[RequireComponent((typeof(Rigidbody)))]
public class Projectile : MonoBehaviour
{
    //We need to have variable public but it is redundant in inspector
    //new will clear the warnings in console
    [HideInInspector] new public Rigidbody rigidbody;

    //This usefull method is called when you attached script to an object for the first time
    void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

}
