using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UDL.Core
{
    public class _Theater
    {
        private static GameObject theater;

        public static T Load<T>(string path) where T : class
        {
            if (theater == null)
                SetUpTheather();

            var prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                throw new System.Exception("Prefab Not Found at: " + path);
            }

            var go = Object.Instantiate(prefab, theater.transform);
            T view = go.GetComponent<T>();

            if (view == null)
            {
                throw new System.Exception("Not found");
            }

            return view;
        }

        public static void AddToTheater(GameObject go)
        {
            if (theater == null)
                SetUpTheather();

            go.transform.SetParent(theater.transform);
        }


        public static GameObject LoadGameObject(string path)
        {
            if (theater == null)
                SetUpTheather();

            var prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
            {
                throw new System.Exception(path + " Not found");
            }

            var go = Object.Instantiate(prefab);

            go.transform.SetParent(theater.transform);

            if (go == null)
            {
                throw new System.Exception("Not found");
            }

            return go;
        }

        private static void SetUpTheather()
        {
            theater = new GameObject("Theater");
            theater.gameObject.AddComponent<Pausable>();
        }

        public static void ClearAll()
        {
            try
            {
                if (theater && theater.gameObject)
                {
                    Object.Destroy(theater.gameObject);
                    theater = null;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public static void Pause(bool pausing)
        {
            if (theater != null)
            {
                theater.GetComponent<Pausable>().pausing = pausing;
            }
        }

        public static void ClearCache()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            Debug.Log("clear cache");
        }
    }
}