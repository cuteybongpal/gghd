using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Catchable : UI_Base, IObserver
{
    public Image _element;
    void Awake()
    {
        _element = FindChild<Image>("Image");
    }

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    public void Notify(Define.NotifyEvent notifyEvent, object value)
    {
        if (_element == null)
            _element = FindChild<Image>("Image");
        switch (notifyEvent)
        {
            case Define.NotifyEvent.Appear:
                _element.gameObject.SetActive(true);
                break;
            case Define.NotifyEvent.Disappear:
                _element.gameObject.SetActive(false);
                break;
        }
    }
}
