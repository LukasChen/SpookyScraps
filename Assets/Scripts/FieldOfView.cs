using UnityEngine;

public class FieldOfView : MonoBehaviour {

    [SerializeField] private LayerMask _layerMask;

    private float _fov = 90f;
    private float _startingAngle = 0;

    private Mesh _mesh;
    
    private void Start() {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void Update() {
        Vector3 origin = Vector3.zero;
        int rayCount = 100;
        float currentAngle = -(_fov / 2);
        float angleIncrement = _fov / rayCount;
        float viewDistance = 20f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] indices = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        int indicesIndex = 0;

        for (int i = 1; i <= rayCount; i++) {
            Vector3 ang2Vec = new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentAngle), 0,
                Mathf.Cos(Mathf.Deg2Rad * currentAngle));
            Vector3 ang2VecGlobal = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (currentAngle + transform.eulerAngles.y)), 0,
                Mathf.Cos(Mathf.Deg2Rad * (currentAngle + transform.eulerAngles.y)));

            Debug.Log(AngleToDirection(transform.eulerAngles.y));

            Vector3 vertex;

            Physics.Raycast(transform.position, ang2VecGlobal, out var hit, viewDistance, _layerMask);
            if (hit.collider == null) {
                vertex = origin + ang2Vec  * viewDistance;
            } else {
                Vector3 localizedPos = hit.point - transform.position;
                vertex = Quaternion.Euler(new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z)) * localizedPos;
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

    private Vector3 AngleToDirection(float angle) =>
        new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0,
                Mathf.Cos(Mathf.Deg2Rad * angle));
}