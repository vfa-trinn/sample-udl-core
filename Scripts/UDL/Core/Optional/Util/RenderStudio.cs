using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UDL.Core;

public class RenderStudio : AbstractView
{
    private static List<RenderStudio> list;

    string label;
    [SerializeField]
    public Camera renderCamera;
    [SerializeField]
    GameObject outerAxis;
    [SerializeField]
    GameObject innerAxis;
    [SerializeField]
    GameObject container;

    private RenderTexture renderTexture;

    [SerializeField]
    public float angleH;
    [SerializeField]
    public float angleV;
    [SerializeField]
    public float distance = -1;
    [SerializeField]
    public float turnSpeed = 0;
    [SerializeField]
    public float fieldOfView = 45;

    public Vector3 containerPosition;

    public static RenderStudio Create(string name = "", RenderTexture rt = null)
    {
        GameObject go = new GameObject("RenderStudio");
        RenderStudio studio = go.AddComponent<RenderStudio>();
        studio.outerAxis = new GameObject("outerAxis");
        studio.outerAxis.transform.SetParent(studio.transform);
        studio.innerAxis = new GameObject("innerAxis");
        studio.innerAxis.transform.SetParent(studio.outerAxis.transform);
        studio.container = new GameObject("container");
        studio.container.transform.SetParent(studio.transform);

        studio.renderCamera = studio.innerAxis.AddComponent<Camera>();

        if (list == null)
            list = new List<RenderStudio>();


        studio.transform.position = new Vector3(list.Count * 1000.0f, -1000.0f, 0);
        list.Add(studio);

        studio.renderCamera.enabled = false;
        studio.name = "RenderStudio - " + name;
        studio.label = name;
        if (rt == null)
        {
            studio.renderTexture = new RenderTexture(1000, 1000, 32);
        }
        else
        {
            studio.renderTexture = rt;
        }
        return studio;
    }

    public static RenderStudio GetStudio(string label)
    {
        if (list == null)
        {
            return null;
        }
        foreach (RenderStudio studio in list)
        {
            if (studio.label == label)
            {
                return studio;
            }
        }
        return null;
    }

    void OnDestroy()
    {
        list.Remove(this);
        renderTexture = null;
    }


    public void SetAngle(float angleH, float angleV)
    {
        this.angleH = angleH;
        this.angleV = angleV;
    }

    public void SetDistance(float distance)
    {
        this.distance = distance;
    }

    public void SetContainerPosition(Vector3 position)
    {
        this.containerPosition = position;
    }

    public void SetTurnSpeed(float turnSpeed)
    {
        this.turnSpeed = turnSpeed;
    }

    public void SetFieldOfView(float fieldOfView)
    {
        this.fieldOfView = fieldOfView;
    }

    public void AddTo(GameObject client)
    {
        this.transform.SetParent(client.transform);
    }

    public void SetLayer(string layer)
    {
        renderCamera.cullingMask = LayerMask.NameToLayer(layer);
    }

    public void AddGameObject(GameObject go, Vector3 position, Vector3 rotation)
    {
        go.transform.SetParent(this.container.transform, false);
        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.Euler(rotation);

    }

    public void AddGameObject(GameObject go)
    {
        AddGameObject(go, Vector3.zero, Vector3.zero);
    }

    public RenderTexture StartCamera()
    {

        this.renderCamera.targetTexture = this.renderTexture;
        this.renderCamera.enabled = true;
        adjustCamera();
        return this.renderTexture;
    }

    public void StopCamera()
    {
        this.renderCamera.enabled = false;
        //Destroy (this.renderTexture);
    }

    public void ClearModels()
    {
        for (int i = 0; i < container.transform.childCount; ++i)
        {
            Debug.Log("model count " + i);
            GameObject.Destroy(container.transform.GetChild(i).gameObject);
        }
    }

    //public void Turn()
    //{
    //    this.container.transform.DOLocalRotate(new Vector3(0, 360 * 2.0f, 0), 2.0f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart);
    //}

    void Update()
    {
        adjustCamera();
    }

    void adjustCamera()
    {
        outerAxis.transform.localRotation = Quaternion.Euler(new Vector3(angleV, angleH, 0));
        innerAxis.transform.localPosition = new Vector3(0, 0, -distance);
        renderCamera.fieldOfView = fieldOfView;

        container.transform.localPosition = containerPosition;

        if (turnSpeed != 0)
        {
            container.transform.localRotation = Quaternion.Euler(new Vector3(0, container.transform.localEulerAngles.y + turnSpeed, 0));
        }
    }

 


}
