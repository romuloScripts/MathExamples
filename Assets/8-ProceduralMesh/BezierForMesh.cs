using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierForMesh : MonoBehaviour
{
    public Transform pIni;
    public Transform pEnd;
    
    [Range(0.01f,1)] public float step = 0.01f;
    [Range(0,1)] public float value=1;

    private void OnDrawGizmos()
    {
        float t = 0;
        var lastPos = GetPos(t);
        
        while (t<1)
        {
            t += step;
            t = Mathf.Clamp01(t);
            var pos = GetPos(t);
            Gizmos.DrawLine(lastPos.pos, pos.pos);
            lastPos = pos;
        }
        
        var current = GetPos(value);
        
        Handles.PositionHandle(current.pos, current.rot);
    }

    public (Vector3 pos, Quaternion rot) GetPos(float t)
    {
        var p1 = pIni.position;
        var p2 = pIni.position + pIni.forward * pIni.localScale.z;
        var p3 = pEnd.position + pEnd.forward * (-1 * pEnd.localScale.z);
        var p4 = pEnd.position;
        
        Vector3 a = Vector3.Lerp(p1, p2, t); 
        Vector3 b = Vector3.Lerp(p2, p3, t);
        Vector3 c = Vector3.Lerp(p3, p4 , t);

        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);


        Vector3 tangent = (bc - ab).normalized;

        Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.Lerp(pIni.up, pEnd.up, t));
        
        return (Vector3.Lerp(ab, bc, t),rotation);
    }
}
