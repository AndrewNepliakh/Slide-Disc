using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;


[CreateAssetMenu(fileName = "GameManager", menuName = "Managers/GameManager")]
public class GameManager : BaseInjectable, IAwake, IStart, IDisable
{
    private int _level;
    private ParticleSystem _confetti;
    private Transform _arena;

    private int _playerBaseItems;
    private int _aiBaseItems;

    private LevelData _levelData;
    private PoolManager _poolManager;
    private EventManager _eventManager;

    private List<BaseItem> _baseItems;

    public void OnAwake()
    {
        _levelData = InjectBox.Get<LevelData>();
        _poolManager = InjectBox.Get<PoolManager>();
        _eventManager = InjectBox.Get<EventManager>();

        _level = 0;
        _arena = GameObject.Find("Arena").GetComponent<Transform>();
        _confetti = GameObject.Find("Confetti").GetComponent<ParticleSystem>();

        _baseItems = new List<BaseItem>();
    }

    public void OnStart()
    {
        InitializeLevel(_level);
    }

    private void InitializeLevel(int level)
    {
        _playerBaseItems = _levelData.GetPlayerBases(level).Count;
        _aiBaseItems = _levelData.GetAiBases(level).Count;

        BuildBases(level);
    }

    private void BuildBases(int level)
    {
        foreach (var baseItemModel in _levelData.GetPlayerBases(level))
        {
            var baseItem = _poolManager.Create<BaseItem>(baseItemModel.prefab, _arena, baseItemModel.position,
                baseItemModel.rotation);
            _baseItems.Add(baseItem);
        }

        foreach (var baseItemModel in _levelData.GetAiBases(level))
        {
            var baseItem = _poolManager.Create<BaseItem>(baseItemModel.prefab, _arena, baseItemModel.position,
                baseItemModel.rotation);
            _baseItems.Add(baseItem);
        }
    }

    public void RemovePlayerBaseItem()
    {
        if (_playerBaseItems > 0) _playerBaseItems--;
    }

    public void RemoveAIBaseItem()
    {
        if (_aiBaseItems > 0) _aiBaseItems--;
    }

    public bool IsLevelOver()
    {
        if (_playerBaseItems <= 0 || _aiBaseItems <= 0)
        {
            _level++;
            var lvlCount = _levelData.GetLevelsCount();
            if (_level == lvlCount) _level = 0;
            
            _eventManager.TriggerEvent<OnLevelOverEvent>();
            _confetti.Play();

            foreach (var baseItem in _baseItems)
            {
                _poolManager.GetPool(typeof(BaseItem)).Deactivate(baseItem);
            }
            
            InitializeLevel(_level);
            
            return true;
        }

        return false;
    }

    public void LocalDisable()
    {
    }
}