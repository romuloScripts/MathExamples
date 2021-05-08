using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTriFunctions : MonoBehaviour
{
    public bool cos;
    public bool sin;
    public bool tan;

    public float length = 20;
    public int resolution = 100;
    public int frequency = 1;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < resolution-1; i++)
        {
            float t =  length * (i / (float) resolution);
            float next = length * ((i+1) / (float) resolution);
            if (cos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(
                    new Vector2(t,Mathf.Cos(t * frequency)), 
                    new Vector2(next,Mathf.Cos(next * frequency))
                    );
            }
            
            if (sin)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(
                    new Vector2(t,Mathf.Sin(t * frequency)), 
                    new Vector2(next,Mathf.Sin(next * frequency))
                );
            }
            
            if (tan)
            {
                Gizmos.color = Color.blue;
                Vector2 p1 = new Vector2(t, Mathf.Tan(t * frequency));
                Vector2 p2 = new Vector2(next,Mathf.Tan(next * frequency));
                if (Vector2.Distance(p1,p2) < 5f)
                {
                    Gizmos.DrawLine(p1, p2);
                }

                
            }
        }
    }
}
