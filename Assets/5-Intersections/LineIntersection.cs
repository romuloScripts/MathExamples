using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIntersection : MonoBehaviour
{
    public enum TestType
    {
        Point,
        Lines
    }
    public TestType testType;
    public Line line1;
    public Line line2;
    public Transform point;
    public bool drawMore;

    private void OnDrawGizmos()
    {
        line1.Draw();
        if (testType == TestType.Point && point)
        {
           Gizmos.DrawSphere(point.position,1); 
           Vector3 closestPoint = line1.ClosestPoint(point.position,drawMore);
           Gizmos.DrawSphere(closestPoint,1); 
        }
        else if (testType == TestType.Lines)
        {
            line2.Draw();
            if (line1.LineLineIntersection(line2, out Vector3 point))
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(point,1); 
            }
        }
    }
}

[Serializable]
public class Line
{
    [SerializeField]private Transform start;
    [SerializeField]private Transform end;

    public Vector3 Start => start.position;
    public Vector3 End => end.position;
    public Vector3 LineVec => End - Start; // Line Vector
    
    // formula linha start + (end - start) * t
    
    public Vector3 ClosestPoint(Vector3 point,bool draw=false) 
    {

        float t = Vector3.Dot(point - Start, LineVec) /
                  Vector3.Dot(LineVec, LineVec); // same sqrMagnitude
        if(draw)
            Gizmos.DrawLine(point, Start + LineVec * t);
       
        t = Mathf.Clamp01(t);
        
        if(draw)
            Gizmos.DrawLine(point, Start + LineVec * t);

        return Start + LineVec * t;
    }

    public float Distance(Vector3 point)
    {
        var closest = ClosestPoint(point);

        return Vector3.Distance(closest, point);
    }
    
    public bool LineLineIntersection(Line line2,out Vector3 intersection)
    {
        // A + vt = B + us
        // vt = B -A + us
        // vt = lineVec3 + us
        Vector3 lineVec3 = line2.Start - Start;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Start, Start + lineVec3);
        
        Gizmos.color = Color.green;
        Vector3 crossVec1and2 = Vector3.Cross(LineVec, line2.LineVec);
        Gizmos.DrawLine(Start, Start + crossVec1and2);
        
        Gizmos.color = Color.red;
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, line2.LineVec);
        Gizmos.DrawLine(line2.Start, line2.Start + crossVec3and2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
        bool coplanar = Mathf.Approximately(planarFactor, 0f);
        bool parallel = Mathf.Approximately(crossVec1and2.sqrMagnitude, 0f);
        
        Debug.Log($"Coplanar {coplanar} Parallel {parallel}");

        //is coplanar, and not parallel
        if(coplanar && !parallel)
        {
            float t = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude; // or vector3.dot(crossVec1and2,crossVec1and2)
            intersection = Start + LineVec * t;
            return true;
        }
        
        intersection = Vector3.zero;
        return false;
    }

    public void Draw()
    {
        Gizmos.DrawLine(Start,End);
    }
}
