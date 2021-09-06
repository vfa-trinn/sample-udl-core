using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UDL.Core;
using UDL.Editor;
using UniRx;
using System;
using System.Text;
using UDL.Core.Sound;

namespace Tests
{
    public class CommonTests
    {
        [UnityTest]
        public IEnumerator CheckMissingAssets()
        {
            var tests = MissingCheck.FindTests();
            var exceptions = new List<string>();
            foreach (var test in tests)
            {
                try
                {                   
                    UnityEngine.Object obj = test.Invoke();
                    var n = obj.name;
                    //Debug.Log(obj.GetType() + " -> " + n);
                }
                catch(Exception e)
                {
                    exceptions.Add(e.ToString());
                }
            }

            for (int i = 0; i < exceptions.Count; i++)
            {
                Debug.LogError("Missing asset exception: " + exceptions[i]);
            }

            Assert.AreEqual(0, exceptions.Count);

            yield return null;
        }
    }
}
