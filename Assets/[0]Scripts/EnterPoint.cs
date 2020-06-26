using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnterPoint : Singleton<EnterPoint>
{
    [SerializeField] private List<BaseInjectable> Injectables = new List<BaseInjectable>();
    private static List<MonoEntity> _monoEntities = new List<MonoEntity>();
    private void Awake()
    {
        foreach (var inject in Injectables)
        {
            InjectBox.Add(inject);
        }
        
        InjectBox.InitializeStartInjectables();
        
        _monoEntities = FindObjectsOfType<MonoEntity>().ToList();
    }

    private void Start()
    {
        foreach (var entity in _monoEntities)
        {
            entity.LocalStart();
        }
    }
    
    private void OnApplicationQuit()
    {
        InjectBox.DisableAll();
    }
}