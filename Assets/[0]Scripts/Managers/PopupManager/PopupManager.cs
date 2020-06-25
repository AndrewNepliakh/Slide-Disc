using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "UIManager", menuName = "Managers/UIManager")]
public class PopupManager : BaseInjectable, IAwake
{
    private Transform _uiParent;
    private PoolManager _poolManager;

    public void OnAwake()
    {
        _uiParent = GameObject.Find("ForeGroundCanvas").GetComponent<Transform>();
        _poolManager = InjectBox.Get<PoolManager>();
    }

    public T ShowPopup<T>(object obj = null) where T : BasePopup
    {
        var popup = _poolManager.GetPool(typeof(T))?.Activate<T>();

        if (popup)
        {
            popup.Show(obj);
            return popup;
        }
        else
        {
            var popupPrefab = Resources.Load<GameObject>("UI/Popups/" + typeof(T));
            var newPopup = _poolManager.Draw<T>(popupPrefab, _uiParent);
            newPopup.Show(obj);
            return newPopup;
        }
    }

    public void ClosePopup<T>(object obj = null) where T : BasePopup
    {
        var popup = GameObject.Find(typeof(T) + "(Clone)").GetComponent<T>();

        if (popup)
        {
            _poolManager.GetPool(typeof(T))?.Deactivate(popup);
            popup.Close();
        }
    }
}