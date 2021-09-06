using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UDL.Editor
{
	public class InitTestSceneRemover
	{
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void Init()
        {
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;

            AssetDatabase.SaveAssets();
        }

        public static void OnPlaymodeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredEditMode)
			{
				var files = Directory.GetFiles(Application.dataPath);
				for (int i = 0; i < files.Length; i++)
				{
					var file = files[i];

					// I'm not sure why we needed this ".meta" check...
					//if (file.Contains("InitTestScene") && !file.EndsWith(".meta", System.StringComparison.Ordinal))
					//{
					//	File.Delete(file);
					//}

					if (file.Contains("InitTestScene") || file.Contains("QuickTestScene"))
					{
						File.Delete(file);
					}
				}
			}
			AssetDatabase.Refresh();
		}
	}
}
