using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Core.UI
{
	public class CanvasCameraSetter : MonoBehaviour
	{

		public static CanvasCameraSetter Instance { get; private set; }

		[SerializeField]
		Camera primaryCamera;
		Canvas canvas;

		void Awake ()
		{
			canvas = this.GetComponent<Canvas> ();
			canvas.worldCamera = primaryCamera;
			Instance = this;
		}

		public void SetCamera (Camera camera)
		{
			canvas.worldCamera = camera;
		}

		public void UnsetCamera ()
		{
			canvas.worldCamera = primaryCamera;
		}

	}
}