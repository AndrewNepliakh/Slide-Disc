using System.Collections;
using Interfaces;
using UnityEngine;


[CreateAssetMenu(fileName = "GameManager", menuName = "Managers/GameManager")]
public class GameManager : BaseInjectable, IAwake, IStart, IDisable
{
    private int _level = 0;
    
    public void OnAwake()
    {

    }

    public void OnStart()
    {

    }

    private void InitializeLevel(int level)
    {

    }


    public void LocalDisable()
    {

    }
}