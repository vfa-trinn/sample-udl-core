using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Core
{
    public interface IWatchfulChild
    {
        void OnParentDestroy();
    }

    public class WatchfulParent : MonoBehaviour
    {
        private void OnDestroy()
        {
            IWatchfulChild[] children = this.GetComponentsInChildren<IWatchfulChild>();
            foreach (var child in children)
            {
                child.OnParentDestroy();
            }
        }
    }
}