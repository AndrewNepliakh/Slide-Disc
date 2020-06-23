using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
   public ParticleSystem PuckParticle => _particleSystem;
   [SerializeField] private ParticleSystem _particleSystem;

   private Vector3 _direction;
   private float _thrust;
   private float _offsetY = 0.15f;

   private bool _isCollide; 

   public void AddForce(Vector3 direction, float thrust)
   {
      _direction = direction + new Vector3(0.0f, _offsetY, 0.0f);
      _thrust = thrust;
   }

   private void Move()
   {
      if (!_isCollide)
      {
         transform.Translate(_direction * _thrust * Time.deltaTime);
      }
   }

   private void BounceChangeDirection()
   {
   }

   private void Update()
   {

      Move();
   }

   private void OnCollisionEnter(Collision other)
   {
      if(other != null) _isCollide = true;
      Debug.Log("Collided: " + other.gameObject.name);
   }
}
