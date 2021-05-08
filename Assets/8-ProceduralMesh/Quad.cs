using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Quad : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _vertices;
	
    private void Start () 
    {
        Generate();
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Quad";

        _vertices = new[]
        {
            new Vector3(-.5f, .5f, 0),
            new Vector3(.5f, .5f, 0),
            new Vector3(-.5f, -.5f, 0),
            new Vector3(.5f, -.5f, 0)
        };
        _mesh.vertices = _vertices;

        int[] triangles =
        {
            2, 0, 1,
            3, 2, 1
        };

        Vector2[] uv = new[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
        };

        _mesh.uv = uv;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }
}
