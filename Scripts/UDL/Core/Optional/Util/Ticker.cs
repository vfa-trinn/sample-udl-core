using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UDL.Core
{
    public class Ticker : IDisposable
    {
        public SimpleSubject Tick = new SimpleSubject();

        float tickPerSecond;

        CompositeDisposable disposable = new CompositeDisposable();

        public Ticker(float tickPerSecond)
        {
            this.tickPerSecond = tickPerSecond;
            Observable.Interval(TimeSpan.FromMilliseconds(tickPerSecond * 1000)).Subscribe(_ =>
            {
                Tick.OnNext();
            }).AddTo(disposable);
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}

