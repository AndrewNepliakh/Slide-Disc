using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterNoiseGenerator : MonoBehaviour
{
    [SerializeField] private float _power = 30.0f;
    [SerializeField] private float _scale = 1.0f;
    [SerializeField] private float _timeScale = 1.0f;

    private float _offsetX;
    private float _offsetY;
    private MeshFilter _meshFilter;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        MakeNoise();
    }

    private void Update()
    {
        MakeNoise();
        _offsetX += Time.deltaTime * _timeScale;
        _offsetY += Time.deltaTime * _timeScale;
        
      
    }
    
    private void MakeNoise()
    {
        Vector3[] vertices = _meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * _power;
        }

        _meshFilter.mesh.vertices = vertices;
    }

    private float CalculateHeight(float x, float y)
    {
        float xCord = x * _scale + _offsetX;
        float yCord = y * _scale + _offsetY;

        return Mathf.PerlinNoise(xCord, yCord);
    }
}
