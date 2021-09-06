using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Editor
{
    public class TemplatePresenterParams
    {
        public string nameSpace = "";
        public string baseName = "";
        public string className => baseName + "Presenter";
    }

    public class PresenterTemplate
    {
        public static string Creatre(TemplatePresenterParams parameters)
        {
            TemplatePresenterParams p = parameters;

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
            text += "    using Model;\n";
            text += "    using View;\n";

            text += "    public class " + p.className + " : AbstractPresenter\n";
            text += "    {\n";
            text += "        public " + p.className + "(" + p.baseName + "Model model, " + p.baseName + "View view) : base(model, view)\n";
            text += "        {\n";
            text += "        }\n";
            text += "    }\n";
            text += "}\n";

            return text;
        }
    }
}