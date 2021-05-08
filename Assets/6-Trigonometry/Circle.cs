using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public float radius=1;
    [Range(0,360)]
    public float angle = 0;
    public bool useTurn;
    [Range(0,1)]
    public float turn = 1;
    public float frequency = 1f;
    public bool drawVector;
    public bool drawCos;
    public bool drawSin;
    public bool drawTangent;

    private const float PI = 3.141593f; //3.14159265 //circunference/diameter
    private const float TAU = 6.2831853f;// 2*PI; //circunference/radius
    private const float Rad2Deg = 180f/PI;
    private const float Deg2Rad = PI/180f;
    private float CircunferenceLength(float r) => 2 * PI * r;

    private void OnDrawGizmos()
    {
        float radians = 0;
        if (Application.isPlaying)
        {
            radians = 2 * PI * Time.time * frequency;
        }
        else
        {
            if(useTurn)
                angle = turn * Mathf.Rad2Deg * 2 * PI;

            radians = angle * Mathf.Deg2Rad;
        }


        float x = Mathf.Cos(radians) * radius;
        float y = Mathf.Sin(radians) * radius;
        float tan = Mathf.Tan(radians); // tan = sin/cos
        
        Vector3 vector = new Vector3(x, y);
        
        Vector3 xVector = new Vector3(x, 0);
        Vector3 yVector = new Vector3(0, y);
        Vector3 tanVector = -Vector2.Perpendicular(vector) * tan;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1,1,0));
        
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        if (drawVector)
        {
            Gizmos.DrawLine(Vector3.zero,vector);
            Gizmos.DrawSphere(vector, 0.025f);
        }

        if (drawCos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero,xVector);
            Gizmos.DrawLine(vector,xVector);
            Gizmos.DrawSphere(xVector, 0.025f);
        }

        if (drawSin)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, yVector);
            Gizmos.DrawLine(vector,yVector);
            Gizmos.DrawSphere(yVector, 0.025f);
        }
        
        if (drawTangent)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(vector,vector + tanVector);
        }
    }
}
