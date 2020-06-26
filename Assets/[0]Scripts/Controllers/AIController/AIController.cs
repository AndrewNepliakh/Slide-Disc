using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IPoolable
{
    private AI _ai;
    private Puck _puck;
    private Rigidbody _aiRigidbody;
    private Rigidbody _puckRigidbody;
    private Vector3 _startPosition = new Vector3(0.0f ,0.0f, 5.0f);

    private EventManager _eventManager;

    private Coroutine _releaseRoutine;

    private float _aiSpeed = 4.0f;
    private float _thrust = 40.0f;
    private float _nimDistToLock = 4.0f;

    private Vector3 _direction;

    private bool _isCaught;
    private bool _puckIsLocked;
    private bool _isReleased;

    private void Start()
    {
        _ai = GetComponent<AI>();
        _puck = GameObject.Find("Puck").GetComponent<Puck>();

        _aiRigidbody = GetComponent<Rigidbody>();
        _puckRigidbody = _puck.GetComponent<Rigidbody>();

        _eventManager = InjectBox.Get<EventManager>();
        _eventManager.Add<OnPuckCollideEvent>((args) => { _isReleased = false; });
        _eventManager.Add<OnPlayerCaughtEvent>((args) => { _isReleased = false;});

        InvokeRepeating(nameof(Wandering), 0.0f, 1.0f);
    }

    public void OnActivate(object argument = default)
    {
        gameObject.SetActive(true);
        _eventManager.Add<OnPuckCollideEvent>((args) => { _isReleased = false; });
        _eventManager.Add<OnPlayerCaughtEvent>((args) => { _isReleased = false;});
    }

    private void Update()
    {
        Move();
        CheckPuckProximity();
    }

    private void Move()
    {
        var puckTransform = _puck.transform;
        var puckPosition = puckTransform.position;
        var iaPosition = _ai.transform.position;

        _ai.transform.LookAt(puckTransform);

        if (!_puckIsLocked) _aiRigidbody.velocity = (puckPosition - iaPosition).normalized * _aiSpeed;
        if (_puck.IsPuckLocked && !_isCaught) _aiRigidbody.velocity = _direction;
        if (_isCaught) _aiRigidbody.velocity = Vector3.zero;

        SetClampCoordinates();
    }

    private void SetClampCoordinates()
    {
        var pos = _ai.transform.position;
        _ai.transform.position =
            new Vector3(Mathf.Clamp(pos.x, -8.0f, 8.0f), 0.0f, Mathf.Clamp(pos.z, 3.0f, 14.0f));
    }

    private void Wandering()
    {
        _direction = new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, Random.Range(-2.0f, 2.0f));
    }

    private void CheckPuckProximity()
    {
        if (!_isReleased)
        {
            var puckPosition = _puck.transform.position;
            var aiPosition = _ai.transform.position;

            if (Vector3.Distance(puckPosition, aiPosition) <= _nimDistToLock)
            {
                _puck.transform.position = aiPosition;

                _puckRigidbody.velocity = Vector3.zero;
                _puckIsLocked = true;
                _isCaught = true;
                if (!_isReleased) ReleasePuck();
                _isReleased = true;
            }
        }

        if (!_puckIsLocked) _ai.transform.LookAt(_puck.transform);
    }

    private void ReleasePuck()
    {
        if (_releaseRoutine == null)
        {
            _releaseRoutine = StartCoroutine(ReleasePuckRoutine());
        }
        else
        {
            StopCoroutine(_releaseRoutine);
            _releaseRoutine = null;
            _releaseRoutine = StartCoroutine(ReleasePuckRoutine());
        }
    }

    private IEnumerator ReleasePuckRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Release();
    }

    private void Release()
    {
        _puckRigidbody.velocity = (GetRandomVector()).normalized * _thrust;
        _isCaught = false;
        _puckIsLocked = false;
        _puck.PuckSide = BaseItemSide.AiBaseItem;
    }

    private Vector3 GetRandomVector()
    {
        return new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, Random.Range(-1.0f, -5.0f));
    }

    public void OnDeactivate(object argument = default)
    {
        gameObject.SetActive(false);
        _eventManager.Remove<OnPuckCollideEvent>((args) => { _isReleased = false; });
        _eventManager.Remove<OnPlayerCaughtEvent>((args) => { _isReleased = false;});
    }
}