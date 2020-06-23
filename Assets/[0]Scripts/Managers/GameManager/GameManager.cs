using System.Collections;
using Interfaces;
using UnityEngine;


[CreateAssetMenu(fileName = "GameManager", menuName = "Managers/GameManager")]
public class GameManager : BaseInjectable, IAwake, IStart, IDisable
{
    private int _level;

    private Transform _arena;

    private LevelData _levelData;
    private PoolManager _poolManager;
    
    public void OnAwake()
    {
        _levelData = InjectBox.Get<LevelData>();
        _poolManager = InjectBox.Get<PoolManager>();

        _level = 0;
        _arena = GameObject.Find("Arena").GetComponent<Transform>();
    }

    public void OnStart()
    {
        InitializeLevel(_level);
    }

    private void InitializeLevel(int level)
    {
        BuildBases(level);
    }

    private void BuildBases(int level)
    {
        foreach (var baseItemModel in _levelData.GetPlayerBases(level))
        {
            _poolManager.Create<BaseItem>(baseItemModel.prefab, _arena, baseItemModel.position, baseItemModel.rotation);
        }
        
        foreach (var baseItemModel in _levelData.GetAiBases(level))
        {
            _poolManager.Create<BaseItem>(baseItemModel.prefab, _arena, baseItemModel.position, baseItemModel.rotation);
        }
    }
    
    public void LocalDisable()
    {

    }
}