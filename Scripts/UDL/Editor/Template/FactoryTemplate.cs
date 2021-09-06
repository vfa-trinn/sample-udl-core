using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Editor
{
    public class TemplateFactoryParams
    {
        public string nameSpace = "";
        public string baseName = "";
        public string className => baseName + "Factory";
        public bool isScreen = false;
        public bool hasContext;
        public bool backwardTransition = false;
        public bool viewDriven = true;
        public bool hasModelAndPresenter = false;
    }

    public class FactoryTemplate
    {
        public static string Creatre(TemplateFactoryParams parameters)
        {
            TemplateFactoryParams p = parameters;
            string prefabDirectory = p.nameSpace.Replace(".", "/").Replace("Project", "");

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
            text += "    using View;\n";

            if (p.hasModelAndPresenter)
            {
                text += "    using Model;\n";
                text += "    using Presenter;\n";
            }

            if (p.hasContext)
            {
                text += "    public class " + p.baseName + "Context\n";
                text += "    {\n";
                text += "        public " + p.baseName + "Context()\n";
                text += "        {\n";
                text += "        }\n";
                text += "    }\n";
            }

            text += "\n";
            text += "    public static class " + p.className + "\n";
            text += "    {\n";

            if (p.hasContext)
            {
                text += "        public static IModel Instantiate(" + p.baseName + "Context context)\n";
            }
            else
            {
                text += "        public static IModel Instantiate()\n";
            }

            text += "        {\n";
            text += "            " + p.baseName + "View view = " + p.baseName + "View.Create();\n";

            if (p.viewDriven && p.hasContext)
            {
                text += "            view.SetContext(context);\n";
            }

            if (p.hasModelAndPresenter)
            {
                text += "            " + p.baseName + "Model model = new " + p.baseName + "Model(" + ((p.viewDriven == false && p.hasContext) ? "context" : "") + ");\n";
                text += "            new " + p.baseName + "Presenter(model, view).AddTo(model.disposables);\n";
            }

            if (p.backwardTransition)
            {
                text += "            Transitioner.AddToHistory(view, () => Instantiate(" + (p.hasContext ? " context" : "") + "));\n";
            }

            text += "\n";
            text += "            return " + ((p.viewDriven) ? "view" : "model") + ";\n";
            text += "        }\n";
            text += "    }\n";
            text += "}\n";

            return text;

        }
    }
}