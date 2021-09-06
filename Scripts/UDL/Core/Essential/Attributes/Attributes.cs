using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Core
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class QuickCheckAttribute : System.Attribute
    {
       
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class DontMissAttribute : System.Attribute
    {

    }

    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class RangedParameterAttribute : System.Attribute
    {
        public int from;
        public int to;
        public int step;

        public RangedParameterAttribute(int from, int to, int step = 1)
        {
            this.from = from;
            this.to = to;
            this.step = step;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class StringArrayParameterAttribute : System.Attribute
    {
        public string[] parameters;

        public StringArrayParameterAttribute(string[] parameters)
        {
            this.parameters = parameters;
        }
    }
}