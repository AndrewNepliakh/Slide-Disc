using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public BaseItemData playerBaseItemData;
    public BaseItemData aiBaseItemData;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
public class LevelData : BaseInjectable
{
    public List<Level> levels;

    public List<BaseItemModel> GetPlayerBases(int level)
    {
        return levels[level].playerBaseItemData.baseItemModels;
    }
    
    public List<BaseItemModel> GetAiBases(int level)
    {
        return levels[level].aiBaseItemData.baseItemModels;
    }

    public int GetLevelsCount()
    {
        return levels.Count;
    }
}