using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TubeMesh : MonoBehaviour
{
    public BezierForMesh bezier;
    public Shape2D shape2D;

    public int segments = 10;
    
    private Mesh _mesh;
    
    private void OnEnable()
    {
        Generate();
    }

    private void Update()
    {
        Generate();
    }

    private void Generate()
    {
        _mesh = new Mesh {name = "Tube"};
        
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        
        var verts2d = shape2D.verts;
        var normals2d = shape2D.normals;
        var lines = shape2D.lines;
        var us = shape2D.us;

        float distance = 0;
        Vector3 prevPos = Vector3.zero;
        float uSpan = shape2D.GetUSpan();

        for (int i = 0; i < segments+1; i++)
        {
            var t = i / (float) segments;
            var pos = bezier.GetPos(t);

            if (i > 0)
            {
                distance += Vector3.Distance(pos.pos, prevPos);
            }
            prevPos = pos.pos;

            for (int j = 0; j < verts2d.Length; j++)
            {
                vertices.Add(transform.InverseTransformPoint(LocalToWorldPos(pos,verts2d[j])));
                normals.Add(transform.InverseTransformDirection(LocalToWorldDir(pos.rot,normals2d[j])));
                uvs.Add(new Vector2(us[j],distance/uSpan));
            }
        }

        for (int i = 0; i < segments; i++)
        {
            int offset = i * verts2d.Length;
            for (int j = 0; j < lines.Length; j+=2)
            {
                int a = offset + lines[j];
                int b = offset + lines[j] + verts2d.Length;
                int c = offset + lines[j + 1];
                int d = offset + lines[j+1] + verts2d.Length;
                
                tris.Add(b);
                tris.Add(a);
                tris.Add(c);
                
                tris.Add(b);
                tris.Add(c);
                tris.Add(d);
            }
        }

        _mesh.SetVertices(vertices);
        _mesh.SetNormals(normals);
        _mesh.SetUVs(0,uvs);
        _mesh.SetTriangles(tris,0);
        //_mesh.RecalculateNormals();
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = _mesh;
    }
    
    private static Vector3 LocalToWorldPos((Vector3 pos, Quaternion rot) pos, Vector3 local)
    {
        return pos.pos + pos.rot * local;
    }
    
    private static Vector3 LocalToWorldDir(Quaternion rot, Vector3 dir)
    {
        return rot * dir;
    }
    
    private float[] GetApproxCurveLength(int precision, BezierForMesh bezier)
    {
        float totalLength = 0;
        float[] samples =  new float[precision];
        Vector3 prev = bezier.pIni.position;
        for (int i = 0; i < samples.Length; i++)
        {
            var t = i / ((float) precision-1);
            Vector3 pos = bezier.GetPos(t).pos;
            totalLength += Vector3.Distance(prev, pos);
            samples[i] = totalLength;
            prev = pos;
        }

        return samples;
    }
    
    private float DistanceInT(float[] points, float t){
        int count = points.Length;
        if(count == 0){
            return 0;
        }
        if(count == 1)
            return points[0];
        float iFloat = t * (count-1);
        int idLower = Mathf.FloorToInt(iFloat);
        int idUpper = Mathf.FloorToInt(iFloat + 1);
        if(idUpper >= count)
            return points[count-1];
        if(idLower < 0)
            return points[0];
        return Mathf.Lerp( points[idLower], points[idUpper], iFloat - idLower);
    }
}
