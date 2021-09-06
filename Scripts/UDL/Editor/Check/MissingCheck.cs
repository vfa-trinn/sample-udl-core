using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UDL.Core;

namespace UDL.Editor
{
    public class MissingCheck
    {
        public static List<Func<UnityEngine.Object>> FindTests()
        {
            Assembly asm = Assembly.Load("Project");

            List<Func<UnityEngine.Object>> tests = new List<Func<UnityEngine.Object>>();

            foreach (Type type in asm.GetTypes())
            {
                var methods = type.GetMethods().ToList().FindAll(x => x.IsStatic);

                foreach (MethodInfo method in methods)
                {
                    var attr = method.GetCustomAttribute<DontMissAttribute>();
                    Type returnType = method.ReturnType;

                    if (attr != null)
                    {
                        tests.Add(() =>
                        {
                            try
                            {
                                int length = method.GetParameters().Length;

                                if (length > 1)
                                {
                                    throw new Exception(type.Name + ": " + "Create method can't tate more than one paramaters");
                                }
                                else if (length == 1)
                                {
                                    ParameterInfo p = method.GetParameters()[0];
                                    if(p.IsOptional == false)
                                    {
                                        throw new Exception(type.Name + ": " + "The parameter of a create method must be optional");
                                    }
                                }

                                object[] parameters = (length == 0) ? null : new object[] { null };

                                var returnedObject = method.Invoke(null, parameters);

                                MonoBehaviour mono = returnedObject as MonoBehaviour;

                                if (mono == null)
                                {
                                    if(returnedObject is UnityEngine.Object obj)
                                    {
                                        return obj;
                                    }
                                    else
                                    {
                                        throw new NullReferenceException(type.Name + "." + method.ToString() + " returns null.");
                                    }
                                }
                                else
                                {
                                    return mono.gameObject;
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning("An error occured in " + type.Name + ": " + e);
                                throw e;
                            }
                        });
                    }
                }
            }

            return tests;
        }
    }
}
