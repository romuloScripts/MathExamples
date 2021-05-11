using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape2D : MonoBehaviour
{
    public Vector2[] verts;
    public Vector2[] normals;
    public int[] lines;
    
    public BezierForMesh bezier;
    
    [Header("Circle")]
    public int circleSegments = 10;
    public float radius = 1;

    private void OnDrawGizmos()
    {
        if (!bezier)
            Gizmos.matrix = transform.localToWorldMatrix;
        
        if(lines== null) return;

        for (int i = 0; i < lines.Length - 1; i++)
        {
            if (bezier)
            {
                var pos = bezier.GetPos(bezier.value);
                var pos1 = LocalToWorldPos(pos, verts[lines[i]]);
                var pos2 = LocalToWorldPos(pos, verts[lines[i+1]]);
                Gizmos.DrawLine(pos1,pos2);
            }else
                Gizmos.DrawLine(verts[lines[i]],verts[lines[i+1]]);
        }
    }

    private static Vector3 LocalToWorldPos((Vector3 pos, Quaternion rot) pos, Vector3 local)
    {
        return pos.pos + pos.rot * local;
    }

    [ContextMenu("GenerateCircle")]
    private void GenerateCircle()
    {
        verts = new Vector2[circleSegments + 1];
        lines = new int[circleSegments*2];
        normals = new Vector2[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            float t = i / (float) (verts.Length - 1);

            float radians = Mathf.PI * 2 * t;

            float x = Mathf.Cos(radians) * radius;
            float y = Mathf.Sin(radians) * radius;

            verts[i] = new Vector2(x, y);
            normals[i] = verts[i].normalized;
        }

        for (int i = 0, vertId = 0; i < lines.Length - 1; i += 2)
        {
            lines[i] = vertId;
            lines[i + 1] = vertId + 1;
            vertId++;
        }
    }
}
