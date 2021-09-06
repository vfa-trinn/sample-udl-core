using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UDL.Editor
{
    public class CreateClasses
    {
        [MenuItem("Assets/UDL/Create a View Class Here", false, 2001)]
        static void CreateViewClass()
        {
            string currentDirectory = GetCurrentDirectory();
            ClassCreator.CreateWindow(currentDirectory, false, false, true, false);
        }

        [MenuItem("Assets/UDL/Create a Factory Here", false, 2002)]
        static void CreateFactory()
        {
            string currentDirectory = GetCurrentDirectory();
            ClassCreator.CreateWindow(currentDirectory, true, true, true, false);
        }


        static string GetCurrentDirectory()
        {
            var flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            var asm = Assembly.Load("UnityEditor.dll");
            var typeProjectBrowser = asm.GetType("UnityEditor.ProjectBrowser");
            var projectBrowserWindow = EditorWindow.GetWindow(typeProjectBrowser);
            return ((string)typeProjectBrowser.GetMethod("GetActiveFolderPath", flag).Invoke(projectBrowserWindow, null)).Replace("Assets/", "");
        }

        

    }
}
