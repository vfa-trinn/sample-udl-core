using System;
using System.Collections.Generic;

namespace UDL.Core
{
    public interface IModalFactory
    {
        AbstractModel Instantiate();
    }

    public interface IScreenFactory
    {
        bool History { get; }
        IModel Instantiate();
    }

    public class ScreenManager
    {
        public static ScreenManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScreenManager();
                return _instance;
            }
        }

        static ScreenManager _instance;

        Stack<IScreenFactory> factories;

        IScreenFactory currentFactory;
        IDisposable currentModel;

        Queue<IScreenFactory> queue;

        public ScreenManager()
        {
            factories = new Stack<IScreenFactory>();
            queue = new Queue<IScreenFactory>();
        }

        public void Reset(IScreenFactory screenFactory)
        {
            factories = new Stack<IScreenFactory>();
            queue = new Queue<IScreenFactory>();
            Transit(screenFactory);
        }

        public void Transit(IScreenFactory screenFactory, bool replace = false)
        {
            if (replace && factories.Count > 0)
            {
                factories.Pop();
            }

            if (screenFactory.History)
            {
                factories.Push(screenFactory);
            }
            Instantiate(screenFactory);
        }

        public void Replace(IScreenFactory screenFactory)
        {
            Transit(screenFactory, true);
        }

        public void Enque(IScreenFactory screenFactory)
        {
            queue.Enqueue(screenFactory);
        }

        public IScreenFactory Dequeue()
        {
            if (queue.Count > 0)
            {
                return queue.Dequeue();
            }
            else
            {
                return factories.Peek();
            }
        }

        public void Back()
        {
            if (currentFactory.History)
            {
                factories.Pop();
            }

            if (factories.Count > 0)
            {
              
                Instantiate(factories.Peek());
            }
        }

        public void Root()
        {
            while (factories.Count > 1)
            {
                factories.Pop();
            }
            Instantiate(factories.Peek());
        }

        public void Reload()
        {
            Instantiate(factories.Peek());
        }

        void Instantiate(IScreenFactory screenFactory)
        {
            DeleteOldLayer();
            currentFactory = screenFactory;
            currentModel = screenFactory.Instantiate();
        }

        void DeleteOldLayer()
        {
            if (currentModel != null)
            {
                currentModel.Dispose();
            }

        }
    }
}

