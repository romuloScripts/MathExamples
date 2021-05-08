using System;
using UnityEngine;

public class QuaternionFormulas : MonoBehaviour
{
    public bool sum;
    public bool debugDot;
    public bool debugQuaternion;
    public bool drawAxis2;
    
    [Header("Quaternion 1")] 
    public Vector3 axis = Vector3.one;
    public float angle = 45;
    
    [Header("Quaternion 2")] 
    public Vector3 axis2 = Vector3.one;
    public float angle2 = 45;
    
    [Header("Lerp")] 
    [Range(0,1)]
    public float t;
    
    private void Update()
    {
        
        var q1 = ToQuaternion(axis,angle);
        var q2 = ToQuaternion(axis2,angle2);

        if (sum)
        {
            transform.rotation = CombineQuaternions(transform.rotation,q1);
        }
        else
        {
            transform.rotation = Slerp(q1, q2, t);
        }
    }

    private Quaternion ToQuaternion(Vector3 axis, float angle)
    {
        // x -> i
        // y -> j
        // z -> k
        //q = w  v
        //q = cos(angle/2)  AxSin(angle/2)

        axis.Normalize();
        float w = Mathf.Cos(angle * Mathf.Deg2Rad / 2f);
        Vector3 v = axis * Mathf.Sin(angle * Mathf.Deg2Rad / 2f);
        Quaternion quaternion = new Quaternion(v.x, v.y, v.z, w);
        return quaternion;
    }

    private static float Dot(Quaternion q1,Quaternion q2)
    {
        return q1.x *  q2.x + q1.y *  q2.y + q1.z *  q2.z +  q1.w *  q2.w;
    }

    private static Quaternion Lerp(Quaternion q1, Quaternion q2, float t)
    {
        Quaternion result = new Quaternion();
        float u = 1 - t;
        result.x = u*q1.x + t*q2.x;
        result.y = u*q1.y + t*q2.y;
        result.z = u*q1.z + t*q2.z;
        result.w = u*q1.w + t*q2.w;
        result.Normalize();
        return result;
    }

    private static Quaternion Slerp(Quaternion q1, Quaternion q2, float t)
    {
        Quaternion result = new Quaternion();
        
        
        float dot = Dot(q1, q2);
        
        if (dot < 0.0f) {
            q2 = new Quaternion(-q2.x,-q2.y,-q2.z,-q2.w);
            dot = -dot;
        }

        float theta0 = Mathf.Acos(dot);
        float sinTheta0 = Mathf.Sqrt(1 - dot * dot);
        //float sinTheta0 = Mathf.Sin(theta0);
        
       // Debug.Log($"Dot: {dot} theta0 {theta0} sinTheta0 {sinTheta0 }");
        
        float u = 1 - t;
        
        float partA = Mathf.Sin(u * theta0) / sinTheta0;
        float partB = Mathf.Sin(t * theta0) / sinTheta0;
        
        //Debug.Log($"partA: {partA} partB: {partB} ");
        
        result.x = partA * q1.x + partB * q2.x;
        result.y = partA * q1.y + partB * q2.y;
        result.z = partA * q1.z + partB * q2.z;
        result.w = partA * q1.w + partB * q2.w;
        
        result.Normalize();

        Debug.Log(result);
        return result;
    }

    private static Quaternion CombineQuaternions(Quaternion q1,Quaternion q2)
    {
        float w1 = q1.w;
        float w2 = q2.w;
        Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
        Vector3 v2 = new Vector3(q2.x, q2.y, q2.z);

        float newW = w1 * w2 - v1.x * v2.x - v1.y * v2.y - v1.z * v2.z;
        float newX = w1 * v2.x + v1.x * w2 + v1.y * v2.z - v1.z * v2.y;
        float newY = w1 * v2.y + v1.y * w2 + v1.z * v2.x - v1.x * v2.z;
        float newZ = w1 * v2.z + v1.z * w2 + v1.x * v2.y - v1.y * v2.x;

        return new Quaternion(newX, newY, newZ, newW);
        //transform.rotation *= q2;
        
    }

    private void OnDrawGizmos()
    {
        Quaternion q1 = ToQuaternion(axis, angle);
        Quaternion q2 = ToQuaternion(axis2, angle2);
        
        Gizmos.DrawLine(transform.position, transform.position + (q1 * Vector3.forward).normalized*1.2f);
        if(drawAxis2)
            Gizmos.DrawLine(transform.position, transform.position + (q2 * Vector3.forward).normalized*1.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + axis.normalized);
        if(drawAxis2)
            Gizmos.DrawLine(transform.position, transform.position + axis2.normalized);

        if (debugDot)
        {

            Gizmos.color = Color.blue;
            Debug.Log($"Dot: {Dot(q1,q2)}");
        }
        else if(debugQuaternion)
        {
            Quaternion q = ToQuaternion(axis, angle);
            Debug.Log($"Q: {q.x} {q.y} {q.z} {q.w}");
        }
    }
    
    
}
