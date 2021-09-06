using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Editor
{
    public class TemplateModelParams
    {
        public string nameSpace = "";
        public string baseName = "";
        public string className => baseName + "Model";
        public bool hasContext = false;
    }

    public class ModelTemplate
    {
        public static string Creatre(TemplateModelParams parameters)
        {
            TemplateModelParams p = parameters;

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
            text += "    public class " + p.className + " : AbstractModel\n";
            text += "    {\n";

            if (p.hasContext)
            {
                text += "        " + p.baseName + "Context context;\n";
                text += "\n";
                text += "        public " + p.className + "(" + p.baseName + "Context context)\n";
                text += "        {\n";
                text += "            this.context = context;\n";
                text += "        }\n";
            }

            text += "    }\n";
            text += "}\n";

            return text;
        }
    }
}
