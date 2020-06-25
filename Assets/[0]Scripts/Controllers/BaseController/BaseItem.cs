using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BaseItemSide
{
    PlayerBaseItem = 0,
    AiBaseItem
}

public class BaseItem : MonoBehaviour, IPoolable
{
    public BaseItemSide BaseItemSide;

    private Coroutine _destroyRoutine;
    private Transform _piecesParent;

    private PoolManager _poolManager;

    private MeshFilter _meshFilter;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Collider _collider;
    private MeshRenderer _meshRenderer;
    private Renderer _renderer;

    public void InitBaseItem()
    {
        _piecesParent = GameObject.Find("[Pieces]").transform;
        _poolManager = InjectBox.Get<PoolManager>();

        _meshFilter = GetComponent<MeshFilter>();
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _renderer = GetComponent<Renderer>();
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    public void DestroyMesh(bool destroy)
    {
        if (_destroyRoutine == null) _destroyRoutine = StartCoroutine(SplitMesh(destroy));
        if (_destroyRoutine != null)
        {
            StopCoroutine(_destroyRoutine);
            _destroyRoutine = null;
            _destroyRoutine = StartCoroutine(SplitMesh(destroy));
        }
    }

    private IEnumerator SplitMesh(bool destroy)
    {
        if (_meshFilter == null || _skinnedMeshRenderer == null)
        {
            yield return null;
        }

        if (_collider)
        {
            _collider.enabled = false;
        }

        Mesh M = new Mesh();
        if (_meshFilter)
        {
            M = _meshFilter.mesh;
        }
        else if (_skinnedMeshRenderer)
        {
            M = _skinnedMeshRenderer.sharedMesh;
        }

        Material[] materials = new Material[0];
        if (_meshRenderer)
        {
            materials = _meshRenderer.materials;
        }
        else if (_skinnedMeshRenderer)
        {
            materials = _skinnedMeshRenderer.materials;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {
            int[] indices = M.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] {0, 1, 2, 2, 1, 0};

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.transform.SetParent(_piecesParent);
                GO.layer = LayerMask.NameToLayer("Particle");
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), 
                    transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
                GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300, 500), explosionPos, 5);
                Destroy(GO, 0.1f + Random.Range(0.0f, 1.0f));
            }
        }

        _renderer.enabled = false;

        yield return new WaitForSeconds(0.5f);
        
        if (destroy == true)
        {
            _poolManager.Remove(this);
            _collider.enabled = true;
            _renderer.enabled = true;
        }
    }

    public void OnActivate(object argument = default)
    {
        gameObject.SetActive(true);
    }

    public void OnDeactivate(object argument = default)
    {
        gameObject.SetActive(false);
    }
}