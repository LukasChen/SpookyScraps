using UnityEngine;
using UnityEngine.Serialization;

public class ConeOfSightRenderer : MonoBehaviour
{
    private static readonly int sViewDepthTexturedID = Shader.PropertyToID("_ViewDepthTexture");
    private static readonly int sViewSpaceMatrixID = Shader.PropertyToID("_ViewSpaceMatrix");

    public Camera ViewCamera;
    public float ViewDistance;
    public float ViewAngle;
    private Material mMaterial;

    [SerializeField] private Material _debugMat;

    private RenderTexture _depthTexture;
    
    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        mMaterial = renderer.material;  // This generates a copy of the material
        renderer.material = mMaterial;

        _depthTexture = new RenderTexture(ViewCamera.pixelWidth, ViewCamera.pixelHeight, 24, RenderTextureFormat.Depth);
        ViewCamera.targetTexture = _depthTexture;
        ViewCamera.farClipPlane = ViewDistance;
        ViewCamera.fieldOfView = ViewAngle;


        transform.localScale = new Vector3(ViewDistance * 2, transform.localScale.y, ViewDistance * 2);

        mMaterial.SetTexture(sViewDepthTexturedID, ViewCamera.targetTexture);
        
        mMaterial.SetFloat("_ViewAngle", ViewAngle);
    }

    private void Update()
    {
        ViewCamera.Render();
        mMaterial.SetMatrix(sViewSpaceMatrixID, ViewCamera.projectionMatrix * ViewCamera.worldToCameraMatrix);
        //WildShit();
    }

    private void WildShit() {
        if (_debugMat != null) {
            _debugMat.SetTexture("_MainTex", _depthTexture);
            _debugMat.mainTexture = _depthTexture;
            Texture2D tex = new Texture2D(ViewCamera.pixelWidth, ViewCamera.pixelHeight, TextureFormat.RGBA32, false);
            RenderTexture.active = _depthTexture;
            tex.ReadPixels(new Rect(0, 0, ViewCamera.pixelWidth, ViewCamera.pixelHeight), 0, 0);
            RenderTexture.active = null;
            byte[] bytes = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes($"{Application.dataPath}/test.png", bytes);
            Debug.Log("yo I tried");
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1f, 0f, 1f));
        Gizmos.DrawWireSphere(Vector3.zero, ViewDistance);
        Gizmos.matrix = Matrix4x4.identity;
    }

#endif
}