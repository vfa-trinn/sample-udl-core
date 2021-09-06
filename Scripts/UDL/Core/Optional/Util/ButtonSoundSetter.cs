using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UDL.Core.Sound;

public class ButtonSoundSetter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    SoundLocalKey key;

    DateTime downDateTime;

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.PlayLocalSound(key);
        downDateTime = DateTime.Now;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TimeSpan span =  DateTime.Now - downDateTime;
        if(span.TotalSeconds > 0.3f)
        {
            SoundManager.PlayLocalSound(key);
        }
    }
}
