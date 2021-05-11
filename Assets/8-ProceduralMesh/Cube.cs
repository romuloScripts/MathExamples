using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    private Mesh _mesh;

    void OnEnable()
    {_mesh = new Mesh {name = "Plane"};
        
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        Generate(0,Quaternion.LookRotation(Vector3.right), vertices,uvs,tris);
        Generate(1,Quaternion.LookRotation(Vector3.left), vertices,uvs,tris);
        Generate(2,Quaternion.LookRotation(Vector3.up), vertices,uvs,tris);
        Generate(3,Quaternion.LookRotation(Vector3.down), vertices,uvs,tris);
        Generate(4,Quaternion.LookRotation(Vector3.forward), vertices,uvs,tris);
        Generate(5,Quaternion.LookRotation(Vector3.back), vertices,uvs,tris);
        
        _mesh.SetVertices(vertices);
        _mesh.SetUVs(0,uvs);
        _mesh.SetTriangles(tris,0);
        _mesh.RecalculateNormals();
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = _mesh;
    }

    void Generate(int planeId,Quaternion rotation, List<Vector3> vertices, List<Vector2> uvs,List<int> tris)
    {
        List<Vector3> newVertices = new List<Vector3>()
        {
            rotation * new Vector3(-0.5f, 0.5f,-0.5f),
            rotation * new Vector3(0.5f, 0.5f,-0.5f),
            rotation * new Vector3(-0.5f, -0.5f,-0.5f),
            rotation * new Vector3(0.5f, -0.5f,-0.5f),
        };

        List<Vector2> newUvs = new List<Vector2>()
        {
            new Vector2(0,1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
        };

        List<int> newTris = new List<int>()
        {
            0+planeId*4, 1+planeId*4, 2+planeId*4,
            3+planeId*4, 2+planeId*4, 1+planeId*4
        };
        
        vertices.AddRange(newVertices);
        uvs.AddRange(newUvs);
        tris.AddRange(newTris);
    }
}
