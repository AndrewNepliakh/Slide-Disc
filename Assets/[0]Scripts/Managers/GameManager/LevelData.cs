using System;
using System.Collections.Generic;
using UnityEngine;


public enum BaseItemType
{
    Cube = 0,
    Rectangle
}

[Serializable]
public class Level
{
    public List<BaseItemType> BaseItemType;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
public class LevelData : BaseInjectable
{
    public List<Level> Levels;
}