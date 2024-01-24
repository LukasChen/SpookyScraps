using System;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    private Mesh _mesh;
    
    private void Start() {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void Update() {
        float fov = 90f;
        Vector3 origin = Vector3.zero;
        int rayCount = 100;
        float currentAngle = 0f;
        float angleIncrement = fov / rayCount;
        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] indices = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        int indicesIndex = 0;

        for (int i = 1; i <= rayCount; i++) {
            Vector3 ang2Vec = new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentAngle), 0,
                Mathf.Cos(Mathf.Deg2Rad * currentAngle));
            Vector3 vertex;

            Physics.Raycast(origin, ang2Vec, out var hit, viewDistance);
            if (hit.collider == null) {
                vertex = origin + ang2Vec  * viewDistance;
            } else {
                vertex = hit.point;
            }
            
            vertices[i] = vertex;

            indices[indicesIndex] = 0;
            indices[indicesIndex + 1] = i - 1;
            indices[indicesIndex + 2] = i;

            indicesIndex += 3;

            currentAngle += angleIncrement;
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = indices;
    }
}