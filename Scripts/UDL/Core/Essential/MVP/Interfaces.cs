using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UDL.Core
{
   
    public interface ICommunicable
    {
        void Communicate(ICommunicable communicatable);
    }

    public class EmptyCommunicable : ICommunicable
    {
        #region ICommunicatable implementation
        public void Communicate(ICommunicable communicatable)
        {
            Debug.Log("You are communicating to an empty communicatable");
        }
        #endregion

    }

    public interface IModel : IDisposable
    {
        SimpleSubject OnDispose { get; }
    }
}