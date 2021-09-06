using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UDL.Core
{
    public class Transitioner
    {
        class TransitionEntity
        {
            public int instanceID;
            public Func<IModel> func;

            public TransitionEntity(MonoBehaviour instance, Func<IModel> func)
            {
                this.instanceID = instance.GetInstanceID();
                this.func = func;
            }
        }

        static Stack<TransitionEntity> entities = new Stack<TransitionEntity>();

        public static void AddToHistory(MonoBehaviour instance, Func<IModel> func)
        {
            entities.Push(new TransitionEntity(instance, func));
        }

        public static void AddToHistory<T>(MonoBehaviour instance, Func<T, IModel> action, T context)
        {
            Func<IModel> func = () =>
            {
                return action(context);
            };
            entities.Push(new TransitionEntity(instance, func));
        }

        public static IModel Back(MonoBehaviour from)
        {
            if (entities.Count > 0)
            {
                TransitionEntity entity = entities.Pop();
                if(from != null && entity.instanceID == from.GetInstanceID())
                {
                    return Back(null);
                }
                return entity.func();
            }
            return null;
        }
    }
}