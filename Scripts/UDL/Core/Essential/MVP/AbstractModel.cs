using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UniRx;
using System.Collections.ObjectModel;

namespace UDL.Core
{
    

    public abstract class AbstractModel : IModel
    {
        public SimpleSubject OnDispose { get; } = new SimpleSubject();
        public CompositeDisposable disposables { get; } = new CompositeDisposable();

        bool disposed = false;

        #region IDisposable implementation
        public virtual void Dispose()
        {
            if (disposed == false)
            {
                disposed = true;
                OnDispose.OnNext();
                disposables.Dispose();
            }
        }
        #endregion


    }
}