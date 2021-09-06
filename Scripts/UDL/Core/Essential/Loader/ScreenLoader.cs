using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UDL.Core.Helper;
using UnityEngine.EventSystems;

namespace UDL.Core
{
	public class ScreenLoader
	{
		private static float maxAspectRatio = 2960.0f / 1440.0f;
		private static LimitedCanvas limitedCanvas;
		private static GameObject background;
		private static Camera camera;

		public static Canvas GetCanvas()
		{
			return limitedCanvas.GetComponent<Canvas>();
		}

		public static Camera GetCamera()
		{
			return camera;
		}

		public static void SetCameraBackgroundColor(Color color)
		{
			if (camera)
			{
				camera.backgroundColor = color;
			}
		}

		public static void SetMaxAspectRatio(float ratio)
		{
			maxAspectRatio = ratio;
		}

		public static Vector2 CanvasRefRez
		{
			get
			{
				return limitedCanvas.GetComponent<CanvasScaler>().referenceResolution;
			}
		}

		public static Vector2 CanvasSize
		{
			get
			{
				return limitedCanvas.GetComponent<RectTransform>().sizeDelta;
			}
		}

		public static Vector3 CanvasScale
		{
			get
			{
				return limitedCanvas.transform.localScale;
			}
		}

		private static void SetUpCanvas()
		{
            Canvas canvasPrefab = Resources.Load<Canvas>("UDL/Canvas");

            if (canvasPrefab == null)
            {
                throw new System.Exception("Create a canvas prefab here: Resources/UDL/Canvas.prefab");
            }

            Canvas canvasGo = Object.Instantiate(canvasPrefab);

            //GameObject canvasGo = new GameObject("Canvas");

            //RectTransform canvasRt = canvasGo.AddComponent<RectTransform>();

            //Canvas canvas = canvasGo.AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            //CanvasScaler canvasScaler = canvasGo.AddComponent<CanvasScaler>();
            //canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            //canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            //canvasScaler.matchWidthOrHeight = 1;

            //GraphicRaycaster graphicRaycaster = canvasGo.AddComponent<GraphicRaycaster>();

            //EventSystem eventSystem = canvasGo.AddComponent<EventSystem>();
            //StandaloneInputModule standaloneInputModule = canvasGo.AddComponent<StandaloneInputModule>();

            limitedCanvas = canvasGo.gameObject.AddComponent<LimitedCanvas>();
			limitedCanvas.SetMaxAspectRatio(maxAspectRatio);

			camera = limitedCanvas.GetComponentInChildren<Camera>();

            if(camera == null)
            {
                var go = new GameObject("Camera");
                camera = go.AddComponent<Camera>();
				camera.depth = int.MinValue;
                go.transform.SetParent(limitedCanvas.transform);
				go.transform.localPosition = new Vector3(0, 0, -10);
            }

			camera.clearFlags = CameraClearFlags.SolidColor;

			GameObject backgroundPrefab = Resources.Load<GameObject>("UDL/Background");

			if (backgroundPrefab != null)
			{
				background = Object.Instantiate(backgroundPrefab);
				limitedCanvas.Insert(background, 1);
			}
		}

		public static T Load<T>(string path, int slot = 3)
		{
			if (limitedCanvas == null)
			{
				SetUpCanvas();
			}

			var prefab = Resources.Load<GameObject>(path);

			if (prefab == null)
			{
				throw new System.Exception(path + " doesn't exist");
			}

			var go = Object.Instantiate(prefab);

			limitedCanvas.Insert(go, slot);

			T view = go.GetComponent<T>();

			return view;
		}


		public static void Reset2D()
		{
			Object.Destroy(limitedCanvas.gameObject);
			Object.Destroy(background.gameObject);
			limitedCanvas = null;
			background = null;
			camera = null;
		}
	}
}