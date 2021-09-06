using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UDL.Core
{
    public class Waiter : IDisposable
    {
        public static readonly float tickInterval = 1 / 30f;

        int tick = 0;
        int requiredTick;
        float tickPerSecond;

        CompositeDisposable disposable = new CompositeDisposable();

        public Waiter(Action completeCallback, Action<float> progressCallback, int requiredTick, float tickPerSecond)
        {
            this.tickPerSecond = tickPerSecond;
            Observable.Interval(TimeSpan.FromMilliseconds(tickInterval * 1000)).Subscribe(_ =>
            {
                tick++;

                progressCallback?.Invoke(((float)tick) / ((float)requiredTick));

                if (tick >= requiredTick)
                {
                    completeCallback();
                    Dispose();
                }
            }).AddTo(disposable);
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}