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
        //List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        
        var verts2d = shape2D.verts;
        var normals2d = shape2D.normals;
        var lines = shape2D.lines;

        for (int i = 0; i < segments+1; i++)
        {
            var t = i / (float) segments;
            var pos = bezier.GetPos(t);

            for (int j = 0; j < verts2d.Length; j++)
            {
                vertices.Add(transform.InverseTransformPoint(LocalToWorldPos(pos,verts2d[j])));
                normals.Add(transform.InverseTransformDirection(LocalToWorldDir(pos.rot,normals2d[j])));
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
        //_mesh.SetUVs(0,uvs);
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
}
