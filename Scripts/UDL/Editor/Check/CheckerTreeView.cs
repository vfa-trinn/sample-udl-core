using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UDL.Editor
{
    public class CheckTreeItem
    {
        public List<string> labelNames;
        public string subfolderName;
        public string displayName;
        public Action action;
        

        public CheckTreeItem(List<string> labelNames, string subfolderName, string displayName, Action action)
        {
            this.labelNames = labelNames;
            this.subfolderName = subfolderName;
            this.displayName = displayName;
            this.action = action;
        }
    }

    public class CheckTreeView : TreeView
    {
        Texture2D subfolderIcon = EditorGUIUtility.FindTexture("blendKey");
        Texture2D playIcon = EditorGUIUtility.FindTexture("PlayButton On");

        List<CheckTreeItem> items;
        Dictionary<int, Action> actionDictionary;

        public CheckTreeView(TreeViewState treeViewState) : base(treeViewState)
        {

        }

        public void AddItemDictionary(List<CheckTreeItem> itemDictionary)
        {
            this.items = itemDictionary;
        }

        protected override TreeViewItem BuildRoot()
        {
            actionDictionary = new Dictionary<int, Action>();

            TreeViewItem root = new TreeViewItem() { id = 0, depth = -1, displayName = "Root" };
            root.children = new List<TreeViewItem>();

            int id = 1;

            foreach (CheckTreeItem item in items)
            {
                TreeViewItem currentParent = root;
                int depth = 0;

                foreach(var label in item.labelNames)
                {
                    if (label == "Check")
                        continue;

                    TreeViewItem newParent = currentParent.children.Find(x => x.displayName == label);

                    if(newParent == null)
                    {
                        newParent = new TreeViewItem() { id = id++, depth = depth, displayName = label };
                        newParent.children = new List<TreeViewItem>();
                        currentParent.children.Add(newParent);
                    }

                    currentParent = newParent;
                    depth++;
                }

                if (item.subfolderName != "")
                {
                    TreeViewItem subfolder = currentParent.children.Find(x => x.displayName == item.subfolderName);
                    if(subfolder == null)
                    {
                        subfolder = new TreeViewItem() { id = id++, depth = depth, displayName = item.subfolderName, icon = subfolderIcon };
                        subfolder.children = new List<TreeViewItem>();
                        currentParent.AddChild(subfolder);
                    }
                    depth++;
                    currentParent = subfolder;
                }


                int actionID = id++;
                TreeViewItem child = new TreeViewItem() { id = actionID, depth = depth, displayName = item.displayName, icon = playIcon };
                currentParent.children.Add(child);
                actionDictionary.Add(actionID, item.action);
            }
            return root;
        }

        protected override void DoubleClickedItem(int id)
        {
            base.DoubleClickedItem(id);
            if (actionDictionary.ContainsKey(id))
            {
                actionDictionary[id].Invoke();
            }
        }
    }
}