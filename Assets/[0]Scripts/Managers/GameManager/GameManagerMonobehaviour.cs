using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMonobehaviour : MonoBehaviour
{
    private GameManager _gameManager;

    private Coroutine _initLevelRoutine;

    public void SetUp(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    

    public void DoCoroutine(IEnumerator coroutine)
    {
        if (_initLevelRoutine == null)
        {
            _initLevelRoutine =  StartCoroutine(coroutine);
        }
        else
        {
            StopCoroutine(_initLevelRoutine);
            _initLevelRoutine = null;
            _initLevelRoutine = StartCoroutine(coroutine);
        }
    }
}
