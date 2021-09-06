using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraCapture
{
    private Camera camera;
    private GameObject container;
    private int renderTextureSize = 1024;
    public SimpleCameraCapture()
    {
        container = new GameObject();
        container.name = "SimpleCameraCapture";
        camera = container.AddComponent<Camera>();
        camera.enabled = false;
    }

    public SimpleCameraCapture SetBackgroundColor(Color color)
    {
        camera.backgroundColor = color;
        return this;
    }

    public SimpleCameraCapture SetClearFlags(CameraClearFlags flags)
    {
        camera.clearFlags = flags;
        return this;
    }

    public SimpleCameraCapture SetPosition(Vector3 position, bool local = true)
    {
        if (local) container.transform.localPosition = position;
        else container.transform.position = position;
        return this;
    }

    public SimpleCameraCapture SetRotation(Vector3 rotation, bool local = true)
    {
        if (local) container.transform.localRotation = Quaternion.Euler(rotation);
        else container.transform.rotation = Quaternion.Euler(rotation);
        return this;
    }

    public SimpleCameraCapture SetParent(Transform parent)
    {
        container.transform.SetParent(parent);
        return this;
    }

    public SimpleCameraCapture SetCullingMask(params string[] layers)
    {
        camera.cullingMask = LayerMask.GetMask(layers);
        return this;
    }

    public SimpleCameraCapture SetCameraRenderMode(bool orthographic, float orthographicSize = 1)
    {
        camera.orthographic = orthographic;
        camera.orthographicSize = orthographicSize;
        return this;
    }

    public Texture2D Capture()
    {
        camera.enabled = true;

        RenderTexture targetRenderTexture = new RenderTexture(renderTextureSize, renderTextureSize, 24);
        targetRenderTexture.Create();

        camera.targetTexture = targetRenderTexture;
        camera.Render();

        Texture2D texture2D = new Texture2D(renderTextureSize, renderTextureSize, TextureFormat.ARGB32, false);

        RenderTexture oldRenderTexture = RenderTexture.active;
        RenderTexture.active = targetRenderTexture;

        Rect photoRect = new Rect(0, 0, targetRenderTexture.width, targetRenderTexture.height);
        texture2D.ReadPixels(photoRect, 0, 0);
        texture2D.Apply();

        RenderTexture.active = oldRenderTexture;
        camera.targetTexture = null;
        camera.enabled = false;
        targetRenderTexture.Release();
        targetRenderTexture = null;
        return texture2D;
    }

    public void Dispose()
    {
        GameObject.Destroy(container);
    }
}