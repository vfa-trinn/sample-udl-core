using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace UDL.Core
{
	public abstract class CommunicableAbstractView : AbstractView
	{
		protected ICommunicable communicable = new EmptyCommunicable();

        public void SetCommunicable(ICommunicable x){
			communicable = x;
		}

        public ICommunicable GetCommunicable()
        {
            return communicable;
        }

        protected void MakeCommunication(CommunicableAbstractView receiver){
			communicable.Communicate (receiver.communicable);
		}
    }


}
