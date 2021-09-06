using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UDL.Editor
{
    public class ClassCreator : EditorWindow
    {
        const string PATH_TO_TEMPLATE = "Scripts/UDL/Editor/Template/";
        const string PREFAB_QUEUE = "prefabInQueue";

        Vector2 scrollPosition = Vector2.zero;

        bool groupMode = false;

        bool creatingView;
        bool creatingFactory;
        bool creatingModelAndPresenter;
        //bool viewDriven;

        int selectedIndex;
        int viewTypeIndex;

        string assetDirectory = "";
        string nameSpace = "";
        string baseName = "";

        TemplateFactoryParams factoryParams = new TemplateFactoryParams();
        TemplateViewParams viewParams = new TemplateViewParams();
        TemplateModelParams modelParams = new TemplateModelParams();
        TemplatePresenterParams presenterParams = new TemplatePresenterParams();

        string factoryAssetPath => assetDirectory + "/" + factoryParams.className + ".cs";
        string checkAssetPath => assetDirectory + "/" + factoryParams.baseName + "Check.cs";
        string viewAssetPath => assetDirectory + (groupMode ? "/View/" : "/") + viewParams.className + ".cs";
        string modelAssetPath => assetDirectory + (groupMode ? "/Model/" : "/") + modelParams.className + ".cs";
        string presenterAssetPath => assetDirectory + (groupMode ? "/Presenter/" : "/") + presenterParams.className + ".cs";

        string editModeTestAssetPath => "Scripts/Tests/EditMode/" + assetDirectory.Replace("Scripts/Project", "") + "/" + baseName + "Test.cs";

        public static ClassCreator CreateWindow(string currentDirectory, bool groupMode, bool factory, bool view, bool modelAndPresenter)
        {
            string[] directories = currentDirectory.Replace("Scripts/", "").Split('/');

            EditorPrefs.DeleteKey(PREFAB_QUEUE);

            ClassCreator window = GetWindowWithRect<ClassCreator>(new Rect(0, 0, 450, 500));
            window.assetDirectory = currentDirectory;
            window.groupMode = groupMode;
            window.creatingFactory = factory;
            window.creatingView = view;
            window.creatingModelAndPresenter = modelAndPresenter;
            window.nameSpace = string.Join(".", directories);
            window.Show();
            return window;
        }

        private void OnGUI()
        {
            bool anyErrors = false;

            GUIStyle bold = new GUIStyle(EditorStyles.label);
            bold.fontStyle = FontStyle.Bold;

            GUIStyle s = new GUIStyle(EditorStyles.label);
            s.normal.textColor = Color.red;
            s.alignment = TextAnchor.MiddleRight;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Namespace");
            nameSpace = EditorGUILayout.TextField(nameSpace);
            EditorGUILayout.EndHorizontal();

            if (nameSpace.StartsWith("Project") == false)
            {
                EditorGUILayout.LabelField("Only allow creating classes in 'Scripts/Project' folder", s);
                anyErrors = true;
            }
            else
            {
                EditorGUILayout.LabelField("");
            }
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Base Name");
            baseName = EditorGUILayout.TextField(Regex.Replace(baseName, @"\s+", ""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (baseName.Length == 0)
            {
                EditorGUILayout.LabelField("Base Name can't be blank or contains a special character.", s);
                anyErrors = true;
            }
            else
            {
                baseName = Regex.Replace(baseName, @"[^0-9a-zA-Z_]+", "");

                var firstChar = baseName.Substring(0, 1);
                if(int.TryParse(firstChar, out int firstNumber))
                {
                    EditorGUILayout.LabelField("Base Name can't accept '" + firstNumber + "' as the prefix.", s);
                    anyErrors = true;
                }
                else if (baseName.ToLower().EndsWith("view"))
                {
                    EditorGUILayout.LabelField("Base Name can't accept 'view' at the suffix.", s);
                    anyErrors = true;
                }
                else
                {
                    EditorGUILayout.LabelField("");
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (groupMode)
            {
                factoryParams.nameSpace = nameSpace;
                factoryParams.baseName = baseName;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(factoryParams.className, bold);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField(factoryAssetPath);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Has Context");
                factoryParams.hasContext = EditorGUILayout.Toggle(factoryParams.hasContext);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Backward Transition");
                factoryParams.backwardTransition = EditorGUILayout.Toggle(factoryParams.backwardTransition);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Mode");
                selectedIndex = EditorGUILayout.Popup(selectedIndex, new string[] { "View Only", "MVP (View-Driven)", "MVP (Model-Driven)" });
                EditorGUILayout.EndHorizontal();

                switch (selectedIndex)
                {
                    case 0:
                        creatingModelAndPresenter = false;
                        factoryParams.hasModelAndPresenter = false;
                        factoryParams.viewDriven = true;
                        break;
                    case 1:
                        creatingModelAndPresenter = true;
                        factoryParams.hasModelAndPresenter = true;
                        factoryParams.viewDriven = true;
                        break;
                    case 2:
                        creatingModelAndPresenter = true;
                        factoryParams.hasModelAndPresenter = true;
                        factoryParams.viewDriven = false;
                        break;
                }

                EditorGUILayout.Space(20);
            }

            {
                viewParams.nameSpace = nameSpace + (groupMode ? ".View" : "");
                viewParams.baseName = baseName;
                viewParams.hasContext = factoryParams.viewDriven && factoryParams.hasContext;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(viewParams.className, bold);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField(viewAssetPath);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                viewTypeIndex = viewParams.is3D ? 0 : 1;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("View Type");
                viewTypeIndex = EditorGUILayout.Popup(viewTypeIndex, new string[] { "3D View", "2D View" });
                EditorGUILayout.EndHorizontal();
                viewParams.is3D = viewTypeIndex == 0;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Create a prefab");
                viewParams.hasPrefab = EditorGUILayout.Toggle(viewParams.hasPrefab);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField(viewParams.hasPrefab ? GetPrefabPath() : "");
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
            }


            if (creatingModelAndPresenter)
            {
                EditorGUILayout.Space(20);

                {
                    modelParams.nameSpace = nameSpace + (groupMode ? ".Model" : "");
                    modelParams.baseName = baseName;
                    modelParams.hasContext = factoryParams.viewDriven == false && factoryParams.hasContext;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(modelParams.className, bold);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.LabelField(modelAssetPath);
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                }

                {
                    presenterParams.nameSpace = nameSpace + (groupMode ? ".Presenter" : "");
                    presenterParams.baseName = baseName;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(presenterParams.className, bold);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.LabelField(presenterAssetPath);
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                }
            }


            EditorGUILayout.Space(20);

            EditorGUILayout.EndScrollView();

            var waitForCompiling = !string.IsNullOrEmpty(EditorPrefs.GetString(PREFAB_QUEUE));

            if (anyErrors || waitForCompiling)
            {
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Button("Can't Create Now");
                EditorGUI.EndDisabledGroup();

                if(waitForCompiling)
                {
                    EditorUtility.DisplayProgressBar("Attaching Script to Prefab Progress", "Waiting for scripts to compiling...", UnityEngine.Random.Range(0f, 0.8f));
                }
            }
            else
            {
                if (GUILayout.Button("Create"))
                {
                    EditorPrefs.DeleteKey(PREFAB_QUEUE);
                    CreateClass();
                }
            }
        }

        private void WriteFile(string assetPath, string text, bool warnIfExists = true)
        {
            string relativePath = "Assets/" + assetPath;
            string absolutePath = Path.Combine(Application.dataPath, assetPath);
            CreateFolder(assetPath);
            if (File.Exists(absolutePath) == false)
            {
                File.WriteAllText(absolutePath, text);
                AssetDatabase.ImportAsset(relativePath);
            }
            else if (warnIfExists)
            {
                Debug.LogWarning("File arleady exists: " + assetPath);
            }
        }

        private void CopyFile(string from, string to)
        {
            var sourcePath = Path.Combine(Application.dataPath, from);
            var template = File.ReadAllText(sourcePath);

            WriteFile(to, template, false);
        }

        private void CreateClass()
        {
            CopyFile(PATH_TO_TEMPLATE + "ProjectDefinition.asmdef.txt", "Scripts/Project/Project.asmdef");
            CopyFile(PATH_TO_TEMPLATE + "PlayModeTestDefinition.asmdef.txt", "Scripts/Tests/PlayMode/PlayModeTest.asmdef");
            CopyFile(PATH_TO_TEMPLATE + "EditModeTestDefinition.asmdef.txt", "Scripts/Tests/EditMode/EditModeTest.asmdef");

            if (creatingView)
            {
                string text = ViewTemplate.Creatre(viewParams);
                WriteFile(viewAssetPath, text);
            }

            if (creatingModelAndPresenter)
            {
                string text = ModelTemplate.Creatre(modelParams);
                WriteFile(modelAssetPath, text);

                text = EditModeTestTemplate.Creatre(modelParams);
                WriteFile(editModeTestAssetPath, text);

                text = PresenterTemplate.Creatre(presenterParams);
                WriteFile(presenterAssetPath, text);
            }

            if (creatingFactory)
            {
                string text = FactoryTemplate.Creatre(factoryParams);
                WriteFile(factoryAssetPath, text);

                text = CheckTemplate.Creatre(factoryParams);
                WriteFile(checkAssetPath, text);
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);

            if (viewParams.hasPrefab)
            {
                CreatePrefab();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            }else
            {
                EditorUtility.DisplayDialog("Information", "Creating Classes Successful.", "OK");
            }
        }

        public string GetPrefabPath()
        {
            return "Resources/" + viewParams.prefabPath + ".prefab";
        }        

        private void CreatePrefab()
        {

            CreateFolder(GetPrefabPath());           

            GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.HideInHierarchy);
            if (viewParams.is3D == false)
            {
                gameObject.AddComponent<RectTransform>();
                gameObject.AddComponent<CanvasRenderer>();
            }

            string typeFullName = nameSpace + (groupMode ? ".View." : ".") + viewParams.className;           

            var prefabSavePath = "Assets/" + GetPrefabPath();

            EditorPrefs.SetString(PREFAB_QUEUE, prefabSavePath + "," + typeFullName);

            PrefabUtility.SaveAsPrefabAsset(gameObject, prefabSavePath);
            DestroyImmediate(gameObject);
        }

        private void CreateFolder(string to)
        {
            to = to.Replace("Assets/", "");
            List<string> targetSegments = to.Split('/').ToList();
            targetSegments.RemoveAt(targetSegments.Count - 1);
            string path = Application.dataPath;
            foreach (string segment in targetSegments)
            {
                path += "/" + segment;
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void AttachScriptWhenReady()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += AttachScriptWhenReady;
                return;
            }

            EditorApplication.delayCall += AttachScriptToPrefab;
        }

        private static void AttachScriptToPrefab()
        {            
            EditorUtility.ClearProgressBar();            

            var prefabInQueue = EditorPrefs.GetString(PREFAB_QUEUE);
            if (string.IsNullOrEmpty(prefabInQueue)) return;

            var keys = prefabInQueue.Split(',');
            var prefabPath = keys[0];
            var scriptToAttach = keys[1];

            if (!string.IsNullOrEmpty(prefabPath) && !string.IsNullOrEmpty(scriptToAttach))
            {
                var prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

                Assembly assembly = Assembly.Load("Project");

                Type t = assembly.GetType(scriptToAttach);

                if (t != null)
                {
                    prefab.AddComponent(t);
                }

                PrefabUtility.SavePrefabAsset(prefab);
                EditorPrefs.DeleteKey(PREFAB_QUEUE);

                EditorUtility.DisplayDialog("Information", "Creating Classes Successful.", "OK");
            }
        }
    }

    
}
