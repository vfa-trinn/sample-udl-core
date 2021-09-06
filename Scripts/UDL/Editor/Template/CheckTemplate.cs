using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Editor
{
    public class CheckTemplate
    {
        public static string Creatre(TemplateFactoryParams parameters)
        {
            TemplateFactoryParams p = parameters;
            string prefabDirectory = p.nameSpace.Replace(".", "/").Replace("Project", "");

            string text = "";

            text += "using System;\n";
            text += "using System.Collections;\n";
            text += "using System.Collections.Generic;\n";
            text += "using UDL.Core;\n";
            text += "using UnityEngine;\n";
            text += "using " + p.nameSpace+ ";\n";
            text += "\n";
            text += "namespace " + p.nameSpace.Replace("Project.", "Check.") + "\n";
            text += "{\n";
            text += "    public class " + p.baseName + "Check\n";
            text += "    {\n";
            text += "        [QuickCheck]\n";
            text += "        public static void Check()\n";
            text += "        {\n";
            if (p.hasContext)
            {
                text += "            " + p.baseName + "Factory.Instantiate(new " + p.baseName + "Context());\n";
            }
            else
            {
                text += "            " + p.baseName + "Factory.Instantiate();\n";
            }
            text += "        }\n";
            text += "    }\n";
            text += "}\n";

            return text;
        }
    }
}