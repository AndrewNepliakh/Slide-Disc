using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public BaseItemSide PuckSide;
    public bool IsPuckLocked;

    public ParticleSystem PuckParticle => _particleSystem;
    [SerializeField] private ParticleSystem _particleSystem;
    
    private readonly Vector3 _puckStartPosition = new Vector3(0.0f, 0.15f, -12.0f);
    private Rigidbody _rigidbody;

    private EventManager _eventManager;
    private GameManager _gameManager;
    private ScoreData _scoreData;

    private void Start()
    {
        _eventManager = InjectBox.Get<EventManager>();
        _gameManager = InjectBox.Get<GameManager>();
        _scoreData = InjectBox.Get<ScoreData>();
        
        transform.position = _puckStartPosition;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        CheckForScorableItem(col);
        DoCollisionWithItem(col);
    }

    private void CheckForScorableItem(Collision collision)
    {
        if (PuckSide == BaseItemSide.PlayerBaseItem)
        {
            var baseItem = collision.gameObject.GetComponent<BaseItem>();
            if (baseItem != null && baseItem.BaseItemSide != PuckSide)
            {
                if (_scoreData.GetItemScoreValue(collision, out int scr))
                {
                    var eventArgs = new OnScoreGainedEvent { Score = _gameManager.GetCurrentUser().AddScores(scr) };
                    _eventManager.TriggerEvent(eventArgs);
                }
            }
        }
    }

    private void DoCollisionWithItem(Collision collision)
    {
        var baseItem = collision.gameObject.GetComponent<BaseItem>();
        
        _eventManager.TriggerEvent<OnPuckCollideEvent>();
        
        if (baseItem)
        {
            if (baseItem.CompareTag("BaseItem") && baseItem.BaseItemSide != PuckSide)
            {
                baseItem.DestroyMesh(true);

                if(baseItem.BaseItemSide == BaseItemSide.PlayerBaseItem) _gameManager.RemovePlayerBaseItem();
                if(baseItem.BaseItemSide == BaseItemSide.AiBaseItem) _gameManager.RemoveAIBaseItem();

                if (_gameManager.IsLevelOver())
                {
                    transform.position = _puckStartPosition;
                    _rigidbody.velocity = Vector3.zero;
                }
            }
        }
    }
    
}