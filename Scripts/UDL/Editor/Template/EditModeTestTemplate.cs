using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Editor
{
    public class EditModeTestTemplate
    {
        public static string Creatre(TemplateModelParams parameters)
        {
            TemplateModelParams p = parameters;

            string text = "";

            text += "using System;\n";
            text += "using System.Collections;\n";
            text += "using System.Collections.Generic;\n";
            text += "using UnityEngine;\n";
            text += "using NUnit.Framework;\n";
            text += "using UDL.Core;\n";
            text += "using " + p.nameSpace.Replace(".Model", "") + ";\n";
            text += "using " + p.nameSpace + ";\n";
            text += "\n";

            text += "namespace " + p.nameSpace.Replace("Project", "Test") + "\n";
            text += "{\n";
            text += "    public class " + p.baseName + "Test\n";
            text += "    {\n";
            text += "        [Test]\n";
            text += "        public void Model()\n";
            text += "        {\n";
            if (p.hasContext)
            {
                text += "            " + p.baseName + "Model model = new " + p.baseName + "Model(new " + p.baseName + "Context());\n";
            }
            else
            {
                text += "            " + p.baseName + "Model model = new " + p.baseName + "Model();\n";
            }
            text += "            Assert.NotNull(model);\n";
            text += "        }\n";
            text += "    }\n";
            text += "}\n";

            return text;
        }
    }
}
