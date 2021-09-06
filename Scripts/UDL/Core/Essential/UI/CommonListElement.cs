using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UDL.Core.UI{
	public class CommonListElement : MonoBehaviour {
		public int id;
		public Button[] buttons;
		public Image[] images;
		public Text[] texts;
        public GameObject[] gameObjects;
		public Toggle[] toggles;
		public Slider[] sliders;
		public RawImage[] rawImages;
		public CommonContainer[] containers;
		public CommonListElement[] elements;
		public CommonListElement[] prefabs;

		public Button button{
			get { return buttons [0];  }	
		}

		public Text text{
			get { return texts [0];  }	
		}

		public Image image{
			get { return images [0];  }	
		}
	}
}
