using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdditional : MonoBehaviour
{
    [SerializeField] Material outlineMat;
    RenderTexture renderTexture;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.R16);
        outlineMat.SetTexture("_OutlineMap", renderTexture);
    }

    void Update()
    {
        Color tempColor = cam.backgroundColor;
        var tempMask = cam.cullingMask;

        cam.targetTexture = renderTexture;  
        cam.cullingMask = 1024;
        cam.backgroundColor = Color.white;
        //cam.clearFlags = CameraClearFlags.SolidColor;
        cam.Render();
        cam.cullingMask = tempMask;
        cam.backgroundColor = tempColor;
        cam.targetTexture = null;
    }

    void OnResize()
    {
        renderTexture.Release();
        Destroy(renderTexture);
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
    }
}
