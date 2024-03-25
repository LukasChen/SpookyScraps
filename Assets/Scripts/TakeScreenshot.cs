using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TakeScreenshot : MonoBehaviour {
    public string pathFolder;
    
    [SerializeField]
    private Camera _cam;

    [ContextMenu("Screenshot")]
    private void ProcessScreenshots() {
        TakeShot($"{Application.dataPath}/{pathFolder}/test.png");
    }

    private void TakeShot(string fullPath) {
        RenderTexture rt = new RenderTexture(256, 256, 24);
        _cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        _cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        _cam.targetTexture = null;
        RenderTexture.active = null;

        if (Application.isEditor) {
            DestroyImmediate(rt);
        } else {
            Destroy(rt);
        }

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
        #if UNITY_EDITOR
        AssetDatabase.Refresh();
        #endif
    }
}
