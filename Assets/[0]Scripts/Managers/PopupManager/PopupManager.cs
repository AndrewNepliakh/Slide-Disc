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

    private List<BasePopup> _allPopups = new List<BasePopup>();

    public void OnAwake()
    {
        _uiParent = GameObject.Find("UI").GetComponent<Transform>();
        _poolManager = InjectBox.Get<PoolManager>();
    }

    public T ShowPopup<T>(object obj = null) where T : BasePopup
    {
        Pool pool = _poolManager.GetPool(typeof(T));
        T popup = null;
        
        if(pool)
            if(pool.GetCount() > 0) popup = _poolManager.GetPool(typeof(T))?.Activate<T>();

        if (popup)
        {
            popup.Show(obj);
            _allPopups.Add(popup);
            return popup;
        }
        else
        {
            var popupPrefab = Resources.Load<GameObject>("UI/Popups/" + typeof(T));
            var newPopup = _poolManager.Draw<T>(popupPrefab, _uiParent);
            newPopup.Show(obj);
            _allPopups.Add(newPopup);
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