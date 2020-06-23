using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public ParticleSystem PuckParticle => _particleSystem;
    [SerializeField] private ParticleSystem _particleSystem;
    
}