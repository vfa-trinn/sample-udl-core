using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Editor
{
    public class TemplateViewParams
    {
        public string nameSpace = "";
        public string baseName = "";
        public string className => baseName + "View";
        public string prefabPath => "Prefabs" + nameSpace.Replace(".", "/").Replace("Project", "").Replace("/View", "") + "/" + className;
        public bool is3D = true;
        public bool hasPrefab = true;
        public bool hasContext;
    }

    public class ViewTemplate
    {
        public static string Creatre(TemplateViewParams parameters)
        {
            TemplateViewParams p = parameters;

            string text = "";

            text += "using System;\n";
            text += "using System.Collections;\n";
            text += "using System.Collections.Generic;\n";
            text += "using UnityEngine;\n";
            text += "using UniRx;\n";
            text += "using UDL.Core;\n";
            text += "\n";

            text += "namespace " + p.nameSpace + "\n";
            text += "{\n";
            text += "    public class " + p.className + " : AbstractView\n";
            text += "    {\n";

            if (p.hasPrefab)
            {                

                if (p.is3D)
                {
                    text += "        [DontMiss]\n";
                    text += "        public static " + p.className + " Create(Transform parent = null)\n";
                    text += "        {\n";
                    text += "            return Instantiate(Resources.Load<" + p.className + ">(\"" + p.prefabPath + "\"), parent);\n";
                    text += "        }\n";
                }
                else
                {
                    text += "        [DontMiss]\n";
                    text += "        public static " + p.className + " Create()\n";
                    text += "        {\n";
                    text += "            return ScreenLoader.Load<" + p.className + ">(\"" + p.prefabPath + "\", 3);\n";
                    text += "        }\n";
                }
                
            }
            else
            {
                text += "        public static " + p.className + " Create()\n";
                text += "        {\n";
                text += "            throw new Exception(\"Error: set view path for this class.\");\n";
                text += "        }\n";
            }

            if (p.hasContext)
            {
                text += "\n";
                text += "        public void SetContext(" + p.baseName + "Context context)\n";
                text += "        {\n";
                text += "        }\n";
            }

            text += "    }\n";
            text += "}\n";

            return text;
        }
    }
}