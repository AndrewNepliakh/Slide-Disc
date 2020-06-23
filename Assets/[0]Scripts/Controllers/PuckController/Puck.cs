using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    private EventManager _eventManager;
    public ParticleSystem PuckParticle => _particleSystem;
    [SerializeField] private ParticleSystem _particleSystem;

    private void Start()
    {
        _eventManager = InjectBox.Get<EventManager>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col != null) _eventManager.TriggerEvent<OnPuckCollideEvent>();
    }
}