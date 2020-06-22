using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonoEntity : MonoBehaviour, IDisable
{
    public static readonly List<MonoEntity> AllUpdates = new List<MonoEntity>();
    protected EventManager EventManager;

    private void OnEnable()
    {
        AllUpdates.Add(this);
        OnTurnOn();
    }

    private void OnDisable()
    {
        AllUpdates.Remove(this);
        OnTurnOff();
    }

    private void Start()
    {
        if(EventManager == null) EventManager = InjectBox.Get<EventManager>();
        OnStart();
    }

    public void LocalStart()
    {
        OnStart();
    }

    public void LocaUpdate()
    {
        OnUpdate();
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnUpdate()
    {
    }

    protected virtual void OnTurnOn()
    {
    }

    protected virtual void OnTurnOff()
    {
    }

    public void LocalDisable()
    {
        OnTurnOff();
    }
}