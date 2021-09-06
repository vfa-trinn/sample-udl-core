using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace UDL.Core
{
	public abstract class AbstractView : MonoBehaviour, IModel
	{
        bool disposed = false;

        #region IDisposable implementation

        public virtual void Dispose ()
		{
            if (disposed == false)
            {
                disposed = true;
                if (this != null)
                {
                    Destroy(this.gameObject);
                }
                OnDispose.OnNext();
            }
		}

		#endregion

        public SimpleSubject OnDispose { get; } = new SimpleSubject();
    }
}
