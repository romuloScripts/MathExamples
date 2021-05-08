using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CatmullRom : MonoBehaviour
{
    [SerializeField]
    private List<Transform> points;
    [Range(0.01f,1)] public float step = 0.01f;
    public bool loop;
    public bool tangents;
    public bool normals;
    [Range(0,1)]
    public float value=1;
    [Range(0,10)]
    public float curve=1f;
    
    public void OnDrawGizmos()
    {
        if(points == null || points.Count<2) return; 
        
        Vector3 pos = points[0].position;
        Vector3 lastPos = Vector3.zero;

        //Gizmos.color = co;

        for (float t = 0.0f; t<1.0f; t+=step) 
        {
            pos = GetPosition(t,true);
            if (t > 0.0f) {
                Gizmos.DrawLine (lastPos,pos);
            }
            lastPos = pos;
        }

        Gizmos.DrawLine (GetPosition(1), lastPos);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetPosition(value,true),0.3f);
    }

    private Vector3 GetPosition(float t,bool draw=false)
    {
        var (p0, p1, tan1, tan2, tBetween2Points) = GetParamsFromGlobalT(t);

        Vector3 pos = CalculatePosition(p0, p1, tan1, tan2, tBetween2Points);

        if (draw)
        {
            Color lastColor = Gizmos.color;
            float dis = 0.8f;
            Vector3 tangent = CalculateTangent(p0, p1, tan1, tan2, tBetween2Points);
            if (tangents)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(pos,pos + tangent* dis);
            }
            
            if (normals)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(pos,pos + NormalFromTangent(tangent)*dis);
            }
            Gizmos.color = lastColor;
        }

        return pos;
    }

    private (Vector3 p0, Vector3 p1, Vector3 tan1, Vector3 tan2, float tBetween2Points) GetParamsFromGlobalT(float t)
    {
        t = Mathf.Clamp01(t);

        int index = Mathf.Clamp(
            Mathf.FloorToInt(t * (points.Count - (loop ? 0 : 1))),
            0, points.Count - (loop ? 1 : 2));

        Vector3 p0 = points[index].position;
        Vector3 p1 = points[(index + 1) % points.Count].position;

        //Tangent M[k] = (P[k+1] - P[k-1]) / 2
        Vector3 tan1 = GetTan1(index, p1, p0);
        Vector3 tan2 = GetTan2(index, p0, p1);
        
        float tBetween2Points = (t == 1f) ? 1f : Mathf.Repeat(t * (points.Count - (loop ? 0 : 1)), 1f);
        
        return (p0, p1, tan1, tan2, tBetween2Points);
        
        Vector3 GetTan1(int i, Vector3 next, Vector3 current)
        {
            Vector3 tan;
            if (i == 0)
            {
                if (loop)
                {
                    tan = next - points[points.Count - 1].position;
                }
                else
                {
                    tan = next - current;
                }
            }
            else
            {
                tan = next - points[i - 1].position;
            }

            return tan * 0.5f * curve;
        }

        Vector3 GetTan2(int i, Vector3 previous, Vector3 current)
        {
            Vector3 tan;
            if (loop)
            {
                if (i == points.Count - 1)
                {
                    tan = points[(i + 2) % points.Count].position - previous;
                }
                else if (i == 0)
                {
                    tan = points[i + 2].position - previous;
                }
                else
                {
                    tan = points[(i + 2) % points.Count].position - previous;
                }
            }
            else
            {
                if (i < points.Count - 2)
                {
                    tan = points[(i + 2) % points.Count].position - previous;
                }
                else
                {
                    tan = current - previous;
                }
            }

            return tan * 0.5f * curve;
        }
    }

    public static Vector3 CalculatePosition(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
    {
        // Hermite curve formula:
        // (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
        Vector3 position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
                           + (t * t * t - 2.0f * t * t + t) * tanPoint1
                           + (-2.0f * t * t * t + 3.0f * t * t) * end
                           + (t * t * t - t * t) * tanPoint2;

        return position;
    }
    
    private Vector3 CalculateTangent(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
    {
        // p'(t) = (6t² - 6t)p0 + (3t² - 4t + 1)m0 + (-6t² + 6t)p1 + (3t² - 2t)m1
        Vector3 tangent = (6 * t * t - 6 * t) * start
                          + (3 * t * t - 4 * t + 1) * tanPoint1
                          + (-6 * t * t + 6 * t) * end
                          + (3 * t * t - 2 * t) * tanPoint2;

        return tangent.normalized;
    }

    private Vector3 NormalFromTangent(Vector3 tangent)
    {
        return Vector3.Cross(tangent, Vector3.up).normalized / 2f;
    }    
}
