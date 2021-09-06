using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace UDL.Core
{
    public interface ISimpleObservable
    {
        IDisposable Subscribe(Action action);
    }

    public class SimpleSubject : ISimpleObservable
    {
        Subject<Unit> hiddenSubject = new Subject<Unit>();

        public void OnNext()
        {
            hiddenSubject.OnNext(Unit.Default);
        }

        public IDisposable Subscribe(Action action)
        {
            return hiddenSubject.Subscribe(_ => action());
        }
    }

    public static class SimpleSubjectExtensions
    {
        public static IDisposable SimpleSubscribe(this Button button, Action action)
        {
            return button.OnClickAsObservable().Subscribe(_ => action());
        }

        public static IDisposable OnClickAsSimpleObservable(this Button button, Action action)
        {
            return button.OnClickAsObservable().Subscribe(_ => action());
        }

        public static SimpleSubject OnClickAsSimpleSubject(this Button button)
        {
            SimpleSubject subject = new SimpleSubject();
            button.OnClickAsObservable().Subscribe(_ => { subject.OnNext(); }).AddTo(button.gameObject);
            return subject;
        }
    }
}
