using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UDL.Core.UI{
    [Serializable]
    public struct NamedGameObject
    {
        public string name;
        public GameObject gameObject;
    }

    public class CommonContainer : MonoBehaviour
	{
		public GameObject[] prefabs;
        public GameObject[] persistentContents;

		public GameObject emptyMessage;

        [NonSerialized]
		public List<GameObject> elements = new List<GameObject> ();
		int count = 0;

		public void Awake()
		{
			if (prefabs != null) {
				foreach (var p in prefabs) {
					p.gameObject.SetActive (false);
				}
			}
        }

		public GameObject InstantiateElement(int index = 0){
			GameObject element = Instantiate (prefabs [index]);
			Add (element.GetComponent<MonoBehaviour>());
			elements.Add (element);
			return element;
		}

        public bool HasKey(string name)
        {
            return prefabs.ToList().Find(x => x.name == name) != null;
        }

        public GameObject InstantiateByName(string name)
        {
            var prefab = prefabs.ToList().Find(x => x.name == name);

            if(prefab == null)
            {
                throw new Exception(name + " could not be found");
            }

            GameObject element = Instantiate(prefab);
            //GameObject element = Instantiate(namedPrefabs.ToList().Find(x => x.name == name).gameObject);
            Add(element.GetComponent<MonoBehaviour>());
            elements.Add(element);
            return element;
        }

        public void Add(MonoBehaviour element){
			element.transform.SetParent (this.transform, false);
			element.gameObject.SetActive (true);

			if (emptyMessage != null)
				emptyMessage.gameObject.SetActive (false);


			count++;

			if (element.GetComponent<GameObject> () != null) {
				elements.Add (element.GetComponent<GameObject> ());
			}
		}

		public void Clear ()
		{
			List<GameObject> persistentList = new List<GameObject> (persistentContents);
			List<GameObject> prefabList = new List<GameObject> ();
			if(prefabs != null)
				prefabList.AddRange (prefabs);
			foreach (Transform t in gameObject.transform)
				if (prefabList.FindIndex(x => x.gameObject == t.gameObject) == -1 && t.gameObject != emptyMessage && persistentList.IndexOf(t.gameObject) == -1 )
					Destroy (t.gameObject);
			if (emptyMessage != null)
				emptyMessage.gameObject.SetActive (true);

			elements = new List<GameObject> ();

			count = 0;
		}

		public int Count{
			get{
				return count;
			}
		}
	}
}
