using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _stretchArrowPrefab;
    [SerializeField] private GameObject _arrowPrefab;

    private Player _player;
    private Puck _puck;
    private Rigidbody _puckRigidbody;
    private StretchArrow _stretchArrow;
    private Arrow _arrow;

    private PoolManager _poolManager;
    private EventManager _eventManager;

    private readonly Vector3 _playerStartPosition = new Vector3(0.0f, 0.0f, -6.0f);

    private bool _isAboveUi;
    private bool _isPuckLocked;
    private bool _isReleased;

    private float _nimDistToLock = 4.0f;
    private float _thrust = 40.0f;

    public TextMeshProUGUI _text1;
    public TextMeshProUGUI _text2;
    public TextMeshProUGUI _text3;
    public TextMeshProUGUI _text4;

    private void Start()
    {
        _eventManager = InjectBox.Get<EventManager>();
        _poolManager = InjectBox.Get<PoolManager>();

        _player = GetComponent<Player>();
        _puck = GameObject.Find("Puck").GetComponent<Puck>();
        
        _puckRigidbody = _puck.GetComponent<Rigidbody>();

        if (_player != null) _player.transform.position = _playerStartPosition;
        
        _stretchArrow = _poolManager.Draw<StretchArrow>(_stretchArrowPrefab, _puck.transform);
        _stretchArrow.gameObject.SetActive(false);

        _arrow = _poolManager.Draw<Arrow>(_arrowPrefab, _player.transform);
        _arrow.gameObject.SetActive(false);

        _eventManager.Add<OnPuckCollideEvent>(OnPuckCollide);
    }

    private void Update()
    {
#if UNITY_EDITOR
        MouseMovementPlayer();
        if (Input.GetMouseButton(0)) PullPuck();
        if (Input.GetMouseButtonUp(0) && _isPuckLocked) ReleasePuck();
        CheckPuckProximity();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        TouchMovementPlayer();
        if (Input.GetMouseButton(0)) PullPuck();
        if (Input.GetMouseButtonUp(0) && _isPuckLocked) ReleasePuck();

       // if (Input.touchCount > 0) PullPuck();
       // if (Input.touches[0].phase == TouchPhase.Ended && _isPuckLocked) ReleasePuck();

        CheckPuckProximity();
#endif
    }


    private void MouseMovementPlayer()
    {
        float deltaX;
        float deltaZ;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) _isAboveUi = true;
            if (!EventSystem.current.IsPointerOverGameObject()) _isAboveUi = false;
        }

        if (Input.GetMouseButtonUp(0)) _isAboveUi = false;

        if (Input.GetMouseButton(0) && !_isAboveUi)
        {
            deltaX = Input.GetAxis("Mouse X");
            deltaZ = Input.GetAxis("Mouse Y");
            _player.transform.position += new Vector3(deltaX, 0.0f, deltaZ);

            SetClampCoordinates();
        }
        else if (!_isAboveUi)
        {
            SetClampCoordinates();
        }
    }

    private void PullPuck()
    {
        var puckPosition = _puck.transform.position;
        var playerPosition = _player.transform.position;

        if (Vector3.Distance(puckPosition, playerPosition) <= _nimDistToLock)
        {
            _isPuckLocked = true;
            _puck.IsPuckLocked = true;
        }

        if (_isPuckLocked)
        {
            _puckRigidbody.velocity = Vector3.zero;
            
            _stretchArrow.gameObject.SetActive(true);
            _stretchArrow.transform.LookAt(_player.transform);
            
            _arrow.gameObject.SetActive(true);
            _player.transform.LookAt(_puck.transform);
            
            var rot = Quaternion.LookRotation(_player.transform.position - _puck.transform.position);
            _player.transform.rotation = new Quaternion(0.0f, rot.y, 0.0f, rot.w);

            var distance = Vector3.Distance(puckPosition, playerPosition);
            var scale = _stretchArrow.transform.localScale;
            
            _stretchArrow.transform.localScale = new Vector3(scale.x, scale.y, distance / 10);

            if (!_puck.PuckParticle.isPlaying) _puck.PuckParticle.Play();
        }
    }
    
    private void ReleasePuck()
    {
        var puckPosition = _puck.transform.position;
        var playerPosition = _player.transform.position;

        _puckRigidbody.velocity = (playerPosition - puckPosition).normalized * _thrust;
        
        
        _stretchArrow.gameObject.SetActive(false);
        _arrow.gameObject.SetActive(false);

        _isPuckLocked = false;
        _puck.IsPuckLocked = false;
        _puck.PuckSide = BaseItemSide.PlayerBaseItem;

        if (_puck.PuckParticle.isPlaying) _puck.PuckParticle.Stop();
        _eventManager.TriggerEvent<OnPlayerCaughtEvent>();
    }
    
    private void CheckPuckProximity()
    {
        if (_isReleased)
        {
            var puckPosition = _puck.transform.position;
            var playerPosition = _player.transform.position;

            if (Vector3.Distance(puckPosition, playerPosition) <= _nimDistToLock)
            {
                if (playerPosition.z > -5.0f)
                    _puck.transform.position = playerPosition + new Vector3(0.0f, 0.0f, -5.0f);
                else
                    _puck.transform.position = playerPosition;
                
                _puckRigidbody.velocity = Vector3.zero;
                _isPuckLocked = true;
                _isReleased = false;
            }
        }

        if (!_isPuckLocked) _player.transform.LookAt(_puck.transform);
    }

    private void SetClampCoordinates()
    {
        var pos = _player.transform.position;
        _player.transform.position =
            new Vector3(Mathf.Clamp(pos.x, -8.0f, 8.0f), 0.0f, Mathf.Clamp(pos.z, -14.0f, -3.0f));
    }

    private void TouchMovementPlayer()
    {
        _text1.text = "TouchMovementPlayer()";

        if (Input.touchCount == 1)
        {
            var firstTouch = Input.GetTouch(0);

            var deltaX = firstTouch.deltaPosition.x * 2.0f * Time.deltaTime;
            var deltaZ = firstTouch.deltaPosition.y * 2.0f * Time.deltaTime;
            
            _player.transform.position += new Vector3(deltaX, 0.0f, deltaZ);

            SetClampCoordinates();
        }
    }

    private void OnPuckCollide(OnPuckCollideEvent args)
    {
        _isReleased = true;
    }

    private void OnDisable()
    {
        _eventManager.Remove<OnPuckCollideEvent>(OnPuckCollide);
    }
}