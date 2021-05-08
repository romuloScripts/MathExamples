using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermit : MonoBehaviour
{
    [Range(0.01f,1)] public float step = 0.01f;
    public Transform p1, p2;
    [Range(0,10)]
    public float scaleTan1 = 1;
    [Range(0,10)]
    public float scaleTan2 = 1;

    private void OnDrawGizmos()
    {
        Verifiy();
        
        float t = 0;
        Vector3 lastPos = GetPos(p1.position,p2.position,p1.forward*scaleTan1,p2.forward*scaleTan2,t);
        while (t<1)
        {
            t += step;
            t = Mathf.Clamp01(t);
            Vector3 pos = GetPos(p1.position,p2.position,p1.forward*scaleTan1,p2.forward*scaleTan2,t);
            Gizmos.DrawLine(lastPos, pos);
            lastPos = pos;
        }
    }

    private Vector3 GetPos(Vector3 p1,Vector3 p2, Vector3 tan1, Vector3 tan2, float t)
    {
        // Hermite curve formula:
        // (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
        return  (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p1 
                + (t * t * t - 2.0f * t * t + t) * tan1 
                + (-2.0f * t * t * t + 3.0f * t * t) * p2 
                + (t * t * t - t * t) * tan2;
    }
    
    GameObject NewTransform(string nome)
    {
        GameObject g = new GameObject(nome);
        g.transform.parent = transform;
        g.transform.localPosition = Vector3.zero;
        return g;
    }
	
    private void Verifiy()
    {
        if(p1 == null){
            p1 = NewTransform("p1").transform;
            p1.position+=Vector3.left*5;
        }
        if(p2 == null){
            p2 = NewTransform("p2").transform;
            p2.position+=Vector3.left*5;
            p2.position+=Vector3.up*7;
        }
    }
}
