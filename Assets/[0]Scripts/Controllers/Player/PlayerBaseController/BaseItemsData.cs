using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Base
{
    public GameObject Prefab;
    public Vector3 Position;
}

[CreateAssetMenu(fileName = "BaseItemData", menuName = "Data/BaseItemData")]
public class BaseItemsData : BaseInjectable
{
    public List<Base> Bases;
}
