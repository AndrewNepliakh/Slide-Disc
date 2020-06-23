using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaseItemSide
{
    PlayerBaseItem = 0,
    AiBaseItem
}

public class BaseItem : MonoBehaviour, IPoolable
{
    public BaseItemSide sideID { get; private set; }
    public void OnActivate(object argument = default)
    {
       gameObject.SetActive(true);
    }

    public void OnDeactivate(object argument = default)
    {
        gameObject.SetActive(false);
    }
}
