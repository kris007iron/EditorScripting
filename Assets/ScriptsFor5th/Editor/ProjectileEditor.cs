using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Projectile))]
public class ProjectileEditor : Editor
{
    void OnSceneGUI()
    {
        //Now we can adjust in scene view the size of damage radius
        var projectile = target as Projectile;
        var transform = projectile.transform;
        projectile.damageRadius = Handles.RadiusHandle(
            transform.rotation,
            transform.position,
            projectile.damageRadius);

    }

    //This code allows us to see projectile without attaching any renderer etc to it also without affecting our game script only in-editor function
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosSelected(Projectile projectile, GizmoType gizmoType)
    {
        Gizmos.DrawSphere(projectile.transform.position, 0.125f);
    }
}
