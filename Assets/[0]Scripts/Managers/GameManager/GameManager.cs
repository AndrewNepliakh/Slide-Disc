﻿using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;


[CreateAssetMenu(fileName = "GameManager", menuName = "Managers/GameManager")]
public class GameManager : BaseInjectable, IAwake, IStart, IDisable
{
    private int _level;
    private ParticleSystem _confetti;
    private Transform _arena;

    private Coroutine _nextLevelRoutine;

    private int _playerBaseItems;
    private int _aiBaseItems;

    private LevelData _levelData;
    private ColorData _colorData;
    
    private PoolManager _poolManager;
    private EventManager _eventManager;
    private GameManagerMonobehaviour _gameManagerMonobehaviour;

    private List<BaseItem> _baseItems;

    public void OnAwake()
    {
        _levelData = InjectBox.Get<LevelData>();
        _colorData = InjectBox.Get<ColorData>();
        
        _poolManager = InjectBox.Get<PoolManager>();
        _eventManager = InjectBox.Get<EventManager>();

        _level = 0;
        _arena = GameObject.Find("Arena").GetComponent<Transform>();
        _confetti = GameObject.Find("Confetti").GetComponent<ParticleSystem>();
        _gameManagerMonobehaviour = GameObject.Find("[EnterPoint]").GetComponent<GameManagerMonobehaviour>();

        _baseItems = new List<BaseItem>();
    }

    public void OnStart()
    {
        _gameManagerMonobehaviour.SetUp(this);
        InitializeLevel(_level);
    }

    public void InitializeLevel(int level)
    {
        _playerBaseItems = _levelData.GetPlayerBases(level).Count;
        _aiBaseItems = _levelData.GetAiBases(level).Count;

        BuildBases(level);
    }

    private void BuildBases(int level)
    {
        BuildBase(_levelData.GetPlayerBases(level), BaseItemSide.PlayerBaseItem, ColorPaletteName.PlayerColor);
        BuildBase(_levelData.GetAiBases(level), BaseItemSide.AiBaseItem, ColorPaletteName.AiColor);
    }

    private void BuildBase(List<BaseItemModel> bases, BaseItemSide itemSide, ColorPaletteName colorPalette)
    {
        foreach (var baseItemModel in bases)
        {
            var baseItem = _poolManager.Draw<BaseItem>(baseItemModel.prefab, _arena, baseItemModel.position,
                baseItemModel.rotation);
            baseItem.InitBaseItem();
            baseItem.BaseItemSide = itemSide;
            baseItem.SetColor(_colorData[colorPalette]);
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
            _gameManagerMonobehaviour.DoCoroutine(InitLevelRoutine(_level));
            _confetti.Play();
            
            return true;
        }

        return false;
    }
    
    private IEnumerator InitLevelRoutine(int level)
    {
        yield return new WaitForSeconds(1.1f);
        
        foreach (var baseItem in _baseItems)
        {
            if(baseItem.gameObject.activeSelf)_poolManager.Remove(baseItem);
        }
        
        _baseItems.Clear();
        
        InitializeLevel(level);
    }
    

    public void LocalDisable()
    {
    }
}