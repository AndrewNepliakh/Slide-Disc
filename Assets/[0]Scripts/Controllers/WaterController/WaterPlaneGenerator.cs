using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterPlaneGenerator : MonoBehaviour
{
    [SerializeField] private float _size = 1.0f;
    [SerializeField] private int _gridSize = 16;

    private MeshFilter _meshFilter;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = GenerateMesh();
    }

    private Mesh GenerateMesh()
    {
        var mesh = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        for (int i = 0; i < _gridSize + 1; i++)
        {
            for (int j = 0; j < _gridSize + 1; j++)
            {
                vertices.Add(new Vector3(-_size * 0.5f + _size * (i / (float)_gridSize), 0.0f, -_size * 0.5f + _size * (j / (float) _gridSize)));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(i / (float)_gridSize, j / (float)_gridSize));
            }
        }

        var triangles = new List<int>();
        var vertCount = _gridSize + 1;

        for (int i = 0; i < vertCount * vertCount - vertCount; i++)
        {
            if ((i + 1) % vertCount == 0)
            {
                continue;
            }
            
            triangles.AddRange(new List<int>
            {
                i,
                i + 1,
                i + vertCount,
                i + vertCount,
                i + 1,
                i + 1 + vertCount
            });
        }
        
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        return mesh;
    }
}
