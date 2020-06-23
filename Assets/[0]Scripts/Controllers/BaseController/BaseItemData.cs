using System;
using System.Collections.Generic;
using UnityEngine;


public enum BaseItemType
{
    Rectangle = 1,
    Stackable2,
    Stackable3,
    Stackable4
}

[Serializable]
public class BaseItemModel
{
    public BaseItemType playerBaseItemType;
    public GameObject prefab;
    public Vector3 position;
    public Vector3 rotation;
}

[CreateAssetMenu(fileName = "PlayerBaseItemData", menuName = "Data/BaseItemData")]
public class BaseItemData : BaseInjectable
{
    public List<BaseItemModel> baseItemModels;
}
