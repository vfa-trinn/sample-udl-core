using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class ComponentExtensions
{
    public static GameObject FindDeep(
        this Component self,
        string name,
        bool includeInactive = false)
    {
        var children = self.GetComponentsInChildren<Transform>(includeInactive);
        foreach (var transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }


    public static List<Material> FindMaterials(
        this Component self,
        bool includeInactive = false,
        bool sharedMaterial = false)
    {
        var materials = new List<Material>();

        var children = self.GetComponentsInChildren<Transform>(includeInactive);
        foreach (var transform in children)
        {
            var renderer = transform.GetComponent<Renderer>();
            if (renderer == null)
                continue;

            Material[] mats;
            if (sharedMaterial)
            {
                mats = renderer.sharedMaterials;
            }
            else
            {
                mats = renderer.materials;
            }

            foreach (var material in mats)
            {
                if (material != null)
                {
                    materials.Add(material);
                }
            }
        }
        return materials;
    }

    public static void SetLayer(this Component self, string layerName, bool includeInactive = true, List<string> ignoreLayerName = null)
    {
        var children = self.GetComponentsInChildren<Transform>(includeInactive);
        foreach (var transform in children)
        {
            if (ignoreLayerName != null)
            {
                if (!ignoreLayerName.Contains(LayerMask.LayerToName(transform.gameObject.layer)))
                {
                    transform.gameObject.layer = LayerMask.NameToLayer(layerName);
                }
            }
            else
            {
                transform.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
    }

    public static void SetTag(this Component self, string tagName, bool includeInactive = true, List<string> ignoreTagName = null)
    {
        var children = self.GetComponentsInChildren<Transform>(includeInactive);
        foreach (var transform in children)
        {
            if (ignoreTagName != null)
            {
                if (!ignoreTagName.Contains(transform.gameObject.tag))
                {
                    transform.gameObject.tag = tagName;
                }
            }
            else
            {
                transform.gameObject.tag = tagName;
            }
        }
    }

    public static void SetActive(this Component obj, bool isActive)
    {
        if (obj != null && obj.gameObject != null)
        {
            if (obj.gameObject.activeSelf != isActive)
            {
                obj.gameObject.SetActive(isActive);
            }
        }
    }

    public static void SetPosition(this GameObject go, Vector3 position, bool isLocal = true)
    {
        if (go != null && go.transform != null)
        {
            SetPosition(go.transform, position);
        }
    }

    public static void SetPosition(this Component obj, Vector3 position, bool isLocal = true)
    {
        if (obj != null && obj.transform != null)
        {
            SetPosition(obj.transform, position);
        }
    }

    public static void SetPosition(this Transform obj, Vector3 position, bool isLocal = true)
    {
        if (obj != null)
        {
            if (isLocal)
            {
                obj.localPosition = position;
            }
            else
            {
                obj.position = position;
            }
        }
    }

    public static void SetScale(this GameObject obj, float scaleFactor)
    {
        if (obj != null && obj.transform != null)
        {
            SetScale(obj.transform, Vector3.one * scaleFactor);
        }
    }

    public static void SetScale(this Transform obj, float scaleFactor)
    {
        if (obj != null)
        {
            SetScale(obj.transform, Vector3.one * scaleFactor);
        }
    }

    public static void SetScale(this GameObject obj, Vector3 scale)
    {
        if (obj != null && obj.transform != null)
        {
            SetScale(obj.transform, scale);
        }
    }

    public static void SetScale(this Transform obj, Vector3 scale)
    {
        if (obj != null)
        {
            obj.localScale = scale;
        }
    }

    public static void SetRotate(this GameObject obj, Vector3 rotate, bool isLocal = true)
    {
        if (obj != null && obj.transform != null)
        {
            SetRotate(obj.transform, rotate, isLocal);
        }
    }

    public static void SetRotate(this Transform obj, Vector3 rotate, bool isLocal = true)
    {
        if (obj != null)
        {
            if (isLocal)
            {
                obj.localRotation = Quaternion.Euler(rotate);
            }
            else
            {
                obj.rotation = Quaternion.Euler(rotate);
            }
        }
    }

    public static void SetText(this Text txt, string content)
    {
        if (txt != null && txt.gameObject != null)
        {
            txt.text = content;
        }
    }

    public static void SetSprite(this Image image, Sprite sprite)
    {
        if (image != null && image.gameObject != null)
        {
            image.sprite = sprite;
        }
    }

    public static void SetText(this Text txt, string format, params string[] content)
    {
        if (txt != null && txt.gameObject != null)
        {
            txt.text = string.Format(format, content);
        }
    }

    public static void SetActive(this Transform obj, bool isActive)
    {
        if (obj != null && obj.gameObject != null)
        {
            if (obj.gameObject.activeSelf != isActive)
            {
                obj.gameObject.SetActive(isActive);
            }
        }
    }

    public static void SetActive(this GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            if (obj.activeSelf != isActive)
            {
                obj.SetActive(isActive);
            }
        }
    }

    public static void SetParentGameObject(this GameObject self, GameObject parent)
    {
        SetParentGameObject(self, parent, Vector3.zero, Vector3.zero, Vector3.one);
    }

    public static void SetParentGameObject(this GameObject self, GameObject parent, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        self.transform.SetParent(parent.transform);
        self.transform.localPosition = position;
        self.transform.localScale = scale;
        self.transform.localRotation = Quaternion.Euler(rotation);
    }
}