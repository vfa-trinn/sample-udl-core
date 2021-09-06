using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UniRx;
using System.Collections.ObjectModel;

namespace UDL.Core
{
    public abstract class CommunicableAbstractModel : AbstractModel, ICommunicable
    {
        #region ICommunicatable implementation

        public virtual void Communicate(ICommunicable communicatable)
        {
        }

        #endregion
    }
}