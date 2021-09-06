using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CollisionWatcher : MonoBehaviour
{
    Subject<Collider> triggerEnterEvent = new Subject<Collider>();
    public IObservable<Collider> TriggerEnterEvent
    {
        get { return triggerEnterEvent; }
    }

    Subject<Collider> triggerExitEvent = new Subject<Collider>();
    public IObservable<Collider> TriggerExitEvent
    {
        get { return triggerExitEvent; }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEnterEvent.OnNext(other);
    }

    private void OnTriggerExit(Collider other)
    {
        triggerExitEvent.OnNext(other);
    }
}
