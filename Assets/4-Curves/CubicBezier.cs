using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicBezier : MonoBehaviour
{
    [Range(0.01f,1)] public float step = 0.01f;
    public Transform p1, p2, p3, p4;
    public bool casteljauAlgorithm;
    [Range(0,1)]
    public float value=1;

    private void OnDrawGizmos()
    {
        VerifiyPoints();


        if (casteljauAlgorithm)
        {
            
            float t = 0;
            Vector3 lastPos = GetPosCasteljau(p1.position,p2.position,p3.position,p4.position,t);
        
            while (t<1)
            {
                t += step;
                t = Mathf.Clamp01(t);
                Vector3 pos = GetPosCasteljau(p1.position,p2.position,p3.position,p4.position,t);
                Gizmos.DrawLine(lastPos, pos);
                lastPos = pos;
            }
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(p1.position, p2.position);
            Gizmos.DrawLine(p2.position, p3.position);
            Gizmos.DrawLine(p3.position, p4.position);
            Vector3 v = GetPosCasteljau(p1.position, p2.position, p3.position, p4.position, value, true);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v,0.3f);
        }
        else
        {
            Gizmos.DrawLine(p1.position, p2.position);
            Gizmos.DrawLine(p3.position, p4.position);
        
            float t = 0;
            Vector3 lastPos = GetPos(p1.position,p2.position,p3.position,p4.position,t);
            
            
        
            while (t<1)
            {
                t += step;
                t = Mathf.Clamp01(t);
                Vector3 pos = GetPos(p1.position,p2.position,p3.position,p4.position,t);
                Gizmos.DrawLine(lastPos, pos);
                lastPos = pos;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetPos(p1.position,p2.position,p3.position,p4.position,value),0.3f);
        }
    }

    private Vector3 GetPos(Vector3 p1,Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        // cubic bezier (1-t)^3*P0 + 3*t*(1-t)^2*P1 + 3*t^2*(1-t)*P2 + t^3*P3
        
        float u = 1-t;
        return p1*u*u*u + p2*3*t*u*u + p3*3*t*t*u + p4*t*t*t;
    }
    
    public Vector3 GetPosCasteljau(Vector3 p1,Vector3 p2, Vector3 p3, Vector3 p4, float t,bool drawGizmos=false)
    {
        Vector3 a = Vector3.Lerp(p1, p2, t); // (1-t) * p1 + p2 *t 
        Vector3 b = Vector3.Lerp(p2, p3, t); // (1-t) * p2 + p3 *t
        Vector3 c = Vector3.Lerp(p3, p4, t); // (1-t) * p3 + p4 *t

        Vector3 ab = Vector3.Lerp(a, b, t); // (1-t) * ((1-t) * p1 + p2 *t) + ((1-t) * p2 + p3 *t) * t
        Vector3 bc = Vector3.Lerp(b, c, t); // (1-t) * ((1-t) * p2 + p3 *t) + ((1-t) * p3 + p4 *t) * t

        if (drawGizmos)
        {
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(ab, bc);
        }
        
        return Vector3.Lerp(ab, bc, t); 
        // (1-t) * ((1-t) * ((1-t) * p1 + p2 *t) + ((1-t) * p2 + p3 *t) * t) + ((1-t) * ((1-t) * p2 + p3 *t) + ((1-t) * p3 + p4 *t) * t) * t
    }
    
    GameObject NewTransform(string nome)
    {
        GameObject g = new GameObject(nome);
        g.transform.parent = transform;
        g.transform.localPosition = Vector3.zero;
        return g;
    }
	
    private void VerifiyPoints()
    {
        if(p1 == null){
            p1 = NewTransform("p1").transform;
            p1.position+=Vector3.left*5;
        }
        if(p2 == null){
            p2 = NewTransform("p2").transform;
            p2.position+=Vector3.left*5;
            p2.position+=Vector3.up*7;
            p2.parent = p1;
        }
        if(p3 == null){
            p3 = NewTransform("p3").transform;
            p3.position+=Vector3.right*5;
            p3.position+=Vector3.up*7;
        }
        if(p4 == null){
            p4 = NewTransform("p4").transform;
            p4.position+=Vector3.right*5;
            p3.parent = p4;
        }
    }
}
