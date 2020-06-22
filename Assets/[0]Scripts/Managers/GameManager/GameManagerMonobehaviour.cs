using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMonobehaviour : MonoBehaviour
{
    private GameManager _gameManager;

    public void SetUp(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void StartInitLevelRoutine()
    {
        StartCoroutine(InitLevelRoutine());
    }

    private IEnumerator InitLevelRoutine()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
