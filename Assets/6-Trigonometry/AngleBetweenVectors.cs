using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleBetweenVectors : MonoBehaviour
{
    public float radius = 1;
    [Range(0,180)]
    public float angleThreshold;
    public Transform target;

    private void OnDrawGizmos()
    {
        Vector2 dir = (target.position- transform.position).normalized;
        
        float dot = Mathf.Clamp(Vector3.Dot(transform.right, dir),-1,1);

        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        bool trigger = angle < angleThreshold && Vector3.Dot(target.position- transform.position,target.position- transform.position) < radius*radius;
        
        DrawArea(trigger);
    }

    private void DrawArea(bool trigger)
    {
        float radians = angleThreshold * Mathf.Deg2Rad;

        Vector3 angleToVector1 = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * radius;
        Vector3 angleToVector2 = new Vector3(Mathf.Cos(-radians), Mathf.Sin(-radians)) * radius;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        if (trigger)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        
        Gizmos.DrawLine(Vector3.zero, angleToVector1);
        Gizmos.DrawLine(Vector3.zero, angleToVector2);

        int count = 100;
        for (int i = 0; i < count-1; i++)
        {
            float t = i / (float )count;
            float next = (i+1) / (float )count;
            float rad = t * angleThreshold * Mathf.Deg2Rad;
            float rad2 = next * angleThreshold * Mathf.Deg2Rad;

            Vector3 vec1Start = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            Vector3 vec2Start = new Vector3(Mathf.Cos(-rad), Mathf.Sin(-rad)) * radius;
            
            Vector3 vec1End = new Vector3(Mathf.Cos(rad2), Mathf.Sin(rad2)) * radius;
            Vector3 vec2End = new Vector3(Mathf.Cos(-rad2), Mathf.Sin(-rad2)) * radius;
            
            Gizmos.DrawLine(vec1Start, vec1End);
            Gizmos.DrawLine(vec2Start, vec2End);

            if (i == count - 2)
            {
                Gizmos.DrawLine(vec1End, angleToVector1);
                Gizmos.DrawLine(vec2End, angleToVector2);
            }
        }

        Gizmos.DrawSphere(Vector3.zero, 0.1f);
    }
}
