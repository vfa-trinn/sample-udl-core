using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UDL.Core;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Experimental.SceneManagement;

namespace UDL.Editor
{
    [Serializable]
    public class TestData
    {
        public string typeName;
        public string methodName;
        public int[] intArgs;
        public string[] stringArgs;
    }

    public class QuickCheckWindow : EditorWindow
    {
        [SerializeField]
        TreeViewState treeViewState;
        CheckTreeView treeView;

        [MenuItem("Tools/Quick Check")]
        public static void Start()
        {
            var assembly = Assembly.Load("UnityEditor.TestRunner");
            var type = assembly.GetType("UnityEditor.TestTools.TestRunner.TestRunnerWindow");
            QuickCheckWindow window = (QuickCheckWindow)EditorWindow.GetWindow<QuickCheckWindow>("Quick Check", type);
            window.Show();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void OnDidReloadScripts()
        {
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        static string currentScenePathKey = "Editor:CurrentScenePath";
        static string currentPrefabPathKey = "Editor:CurrentPrefabPath";
        static string jsonKey = "Editor:TestDataJson";

        private void OnEnable()
        {
            if (treeViewState == null)
            {
                treeViewState = new TreeViewState();
            }
            Refresh();
        }

        void OnGUI()
        {
            if (treeView != null)
            {
                treeView.OnGUI(new Rect(0, 0, position.width, position.height));
            }
        }

        private void OnFocus()
        {
            Refresh();
        }

        void Refresh()
        {
            treeView = new CheckTreeView(treeViewState);

            Assembly asm = Assembly.Load("Project");

            List<CheckTreeItem> items = new List<CheckTreeItem>();

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == null)
                    continue;

                List<string> labelNames = type.Namespace.Split('.').ToList();

                var methods = type.GetMethods().ToList().FindAll(x => x.IsStatic);

                foreach (MethodInfo method in methods)
                {
                    var attr = method.GetCustomAttribute<QuickCheckAttribute>();

                    if (attr != null)
                    {
                        var parameters = method.GetParameters();

                        if (parameters.Length > 1)
                        {
                            throw new Exception("Quick Check can't take more than one arguments.");
                        }
                        else if (parameters.Length == 1)
                        {
                            foreach (var parameter in parameters)
                            {
                                IEnumerable<CustomAttributeData> attributes = parameter.GetCustomAttributesData();
                                foreach (var attrData in attributes)
                                {
                                    if (attrData.AttributeType == typeof(RangedParameterAttribute))
                                    {
                                        int from = (int)attrData.ConstructorArguments[0].Value;
                                        int to = (int)attrData.ConstructorArguments[1].Value;
                                        int step = attrData.ConstructorArguments.Count >= 2 ? (int)attrData.ConstructorArguments[2].Value : 1;

                                        for (int i = from; i <= to; i += step)
                                        {
                                            int param = i;
                                            items.Add(new CheckTreeItem(labelNames, method.Name, method.Name + "(" + param + ")", () =>
                                            {
                                                OnButtonDown(type, method, new int[1] { param });
                                            }));
                                        }
                                    }
                                    else if(attrData.AttributeType == typeof(StringArrayParameterAttribute))
                                    {
                                        object anArray = attrData.ConstructorArguments[0].Value;
                                        IEnumerable enumerable = anArray as IEnumerable;
                                        if (enumerable != null)
                                        {
                                            foreach (object element in enumerable)
                                            {
                                                string param = Convert.ToString(element).Trim('"');
                                                items.Add(new CheckTreeItem(labelNames, method.Name, method.Name + "(" + param + ")", () =>
                                                {
                                                    OnButtonDown(type, method, null, new string[1] { param });
                                                }));
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            items.Add(new CheckTreeItem(labelNames, "", method.Name, () =>
                            {
                                OnButtonDown(type, method);
                            }));
                        }
                    }
                }

            }

            treeView.AddItemDictionary(items);
            treeView.Reload();
        }

        void OnButtonDown(Type type, MethodInfo method, int[] intArgs = null, string[] stringArgs = null)
        {
            TestData testData = new TestData();
            testData.typeName = type.FullName;
            testData.methodName = method.Name;
            testData.intArgs = intArgs;
            testData.stringArgs = stringArgs;

            PlayerPrefs.SetString(jsonKey, JsonUtility.ToJson(testData));            
                        
            if (!EditorApplication.isPlaying)
            {
                Scene currentScene = EditorSceneManager.GetActiveScene();
                PlayerPrefs.SetString(currentScenePathKey, currentScene.path);                

                var currentPrefab = PrefabStageUtility.GetCurrentPrefabStage();
                PlayerPrefs.SetString(currentPrefabPathKey, currentPrefab == null ? "" : currentPrefab.prefabAssetPath);

                Scene testScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                string path = "Assets/QuickTestScene-" + UnityEngine.Random.Range(100000000, 999999999) + ".unity";
                EditorSceneManager.SaveScene(testScene, path);
                EditorSceneManager.OpenScene(path);
                EditorApplication.isPlaying = true;
            }
            else
            {
                Debug.LogError("Please stop the current Scene before running this check.");
            }
        }

        static void OnPlaymodeStateChanged(PlayModeStateChange state)
        {            
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                string currentScenePath = PlayerPrefs.GetString(currentScenePathKey, "");
                if (currentScenePath != "")
                {
                    try
                    {
                        EditorSceneManager.OpenScene(currentScenePath);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("We can't find the previous scene, but never mind. :" + e);
                    }
                    PlayerPrefs.SetString(currentScenePathKey, "");
                }

                var currentPrefabPath = PlayerPrefs.GetString(currentPrefabPathKey);
                if(string.IsNullOrEmpty(currentPrefabPath) == false)
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(currentPrefabPath));
                    PlayerPrefs.SetString(currentPrefabPathKey, "");
                }
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {
                TestData testData = JsonUtility.FromJson<TestData>(PlayerPrefs.GetString(jsonKey, ""));
                PlayerPrefs.SetString(jsonKey, "");

                if (testData == null)
                    return;

                Assembly asm = Assembly.Load("Project");

                Type type = asm.GetType(testData.typeName);
                ConstructorInfo con = type.GetConstructor(Type.EmptyTypes);
                MethodInfo mi = type.GetMethod(testData.methodName);

                object[] args = null;

                if (testData.intArgs != null && testData.intArgs.Length > 0)
                {
                    args = new object[testData.intArgs.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        args[i] = testData.intArgs[i];
                    }
                }
                else if(testData.stringArgs != null && testData.stringArgs.Length > 0)
                {
                    args = new object[testData.stringArgs.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        args[i] = testData.stringArgs[i];
                    }
                }

                object returnedValue = mi.Invoke(null, args);

                if (mi.ReturnType == typeof(IEnumerator))
                {
                    CoroutineHandler.StartStaticCoroutine(returnedValue as IEnumerator);
                }
            }
        }

    }
}