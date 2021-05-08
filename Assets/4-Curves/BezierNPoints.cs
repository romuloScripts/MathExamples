using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierNPoints : MonoBehaviour{
	
    [Range(0.01f,1)] public float step = 0.01f;
    public Transform[] points;
    public bool casteljau;
    [Range(0,1)]
    public float value=1; 
    
    int[ , ] pascal;

    private void OnDrawGizmos()
    {
        if(points.Length <= 0) return;

        if(!casteljau)
            CalcPascalTriangle ();
        
        Vector3 lastPos = Vector3.zero;

        for (float t = 0.0f; t<1.0f; t+=step) 
        {
            Vector3 pos = casteljau? CalcCasteljau(t) : CalcBezier (t);
		
            if (t > 0.0f) {
                Gizmos.DrawLine (pos, lastPos);
            }
        
            lastPos = pos;
        }
    
        Gizmos.DrawLine (casteljau? CalcCasteljau(1) : CalcBezier (1), lastPos);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(casteljau? CalcCasteljau(value,true) : CalcBezier (value),0.3f);
    }

    public Vector3 CalcCasteljau(float t,bool drawGizmos=false)
    {
        int n = points.Length;

        Vector3[] positions = new Vector3[n];

        for (int i = 0; i <  points.Length; i++)
        {
            positions[i] = points[i].position;
        }

        while (n > 1)
        {
            --n;
            for (int i = 0; i < n; i++)
            {
                if(drawGizmos)
                    Gizmos.DrawLine(positions[i], positions[i + 1]);
                //Formula: Pi = Pi * (1-t) + Pi+1 * t
                positions[i] = positions[i] * (1.0f - t) + positions[i + 1] * t;
            }
        }

        return positions[0];
    }

    public Vector3 CalcBezier(float t, int index=0)
    {
        if (index > points.Length - 1)
            return Vector3.zero;

        return pascal [points.Length - 1, index] * 
               Mathf.Pow (t, index) *
               Mathf.Pow (1 - t, points.Length - 1 - index) * 
               points[index].transform.position
               + CalcBezier (t, index + 1);
    }
	
    public void CalcPascalTriangle()
    {
        pascal = new int[points.Length + 1, points.Length + 1];
        int n = 0;
        int column = 0;
		
        for (int i = column; i< pascal.GetLength(0); i++)
        {
            for (int j = n; j< pascal.GetLength(0); j++){
                pascal [j, i++] = 1; 
            }
            n ++;
            column++;
        }
        
        for (int i = column; i< pascal.GetLength(0); i++) 
        {	
            pascal [i, 0] = 1;
        }
			
        for (int i = 2; i< pascal.GetLength(0); i++) 
        {	
            for (int j = 1; j< pascal.GetLength(1)-1; j++) 
            {	
                pascal [i, j] = pascal [i - 1, j - 1] + pascal [i - 1, j];
            }	
        }	
    }
}