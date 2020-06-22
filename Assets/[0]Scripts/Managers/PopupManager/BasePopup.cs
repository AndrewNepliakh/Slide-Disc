using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour, IPoolable
{
    protected PoolManager PoolManager;
    protected EventManager EventManager;
    protected PopupManager PopupManager;
    public void Show(object obj = null)
    {
        PoolManager = InjectBox.Get<PoolManager>();
        EventManager = InjectBox.Get<EventManager>();
        PopupManager = InjectBox.Get<PopupManager>();
        
        
        OnShow(obj);
    }

    public void Close()
    {
        OnClose();
    }

    
    protected virtual void OnShow(object obj = null){}
    protected virtual void OnClose(){}

    public virtual void OnActivate(object argument = default)
    {
        gameObject.SetActive(true);
    }

    public virtual void OnDeactivate(object argument = default)
    {
        gameObject.SetActive(false);
    }

}
